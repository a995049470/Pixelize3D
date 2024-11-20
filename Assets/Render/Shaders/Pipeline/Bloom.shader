Shader "LPipeline/Bloom"
{
	Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_Offset("模糊半径", Range(0, 8)) = 0 
        _Threshold("Threshold", Range(0, 2)) = 1
        _SoftThreshold("SoftThreshold", Range(0, 1)) = 0
        _Intensity("Intensity", Range(0, 10)) = 1
        
    }
	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	//float4 _MainTex_ST;
    float4 _MainTex_TexelSize;
    Texture2D _MainTex; SamplerState sampler_MainTex;
	half _Offset;
    half _Threshold;
    half _SoftThreshold;
    half _Intensity;
	// Texture2D _MaskTex; SamplerState sampler_MaskTex;
	// float _WeakPow; float _Strength;
	
    struct appdata
    {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;
    };


	struct v2f_DownSample
	{
		float4 vertex: SV_POSITION;
		float2 texcoord: TEXCOORD0;
		float2 uv: TEXCOORD1;
	};
    
	
	
	struct v2f_UpSample
	{
		float4 vertex: SV_POSITION;
		float2 texcoord: TEXCOORD0;
		float4 uv01: TEXCOORD1;
		float4 uv23: TEXCOORD2;
		float4 uv45: TEXCOORD3;
		float4 uv67: TEXCOORD4;
	};

    struct v2f
	{
		float4 vertex: SV_POSITION;
		float2 uv: TEXCOORD1;
	};
	
	
	v2f_DownSample Vert_DownSample(appdata v)
	{
		v2f_DownSample o;
		o.vertex = TransformObjectToHClip(v.vertex);
		o.texcoord = v.texcoord;
		o.uv = o.texcoord;
		
		return o;
	}

    v2f Vert(appdata v)
	{
		v2f o;
		o.vertex = TransformObjectToHClip(v.vertex);
		o.uv = v.texcoord;
		
		return o;
	}

    half4 Frag_Filter(v2f i) : SV_TARGET
    {
        half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
        float brightness = max(color.r, max(color.g, color.b));
        half knee = _Threshold * _SoftThreshold;
        half soft = brightness - _Threshold + knee;
        soft = clamp(soft, 0, 2 * knee);
        soft = soft * soft / (4 * knee + 0.00001);
        half contribution = max(soft, brightness - _Threshold);
        return color * contribution;
    }
    
    half4 Frag_ApplyBloom(v2f i) : SV_TARGET
    {
        half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
        color *= _Intensity;
        return color;
    }
	
	half4 Frag_DownSample(v2f_DownSample i): SV_Target
	{
		float2 uv = i.uv;
		float4 texelSize = _MainTex_TexelSize * 0.5;
		float2 offset = float2(_Offset, _Offset);
		float4 uv01 = 0;
		float4 uv23 = 0;
		uv01.xy = uv - texelSize * offset;//top right
		uv01.zw = uv + texelSize * offset;//bottom left
		uv23.xy = uv - float2(texelSize.x, -texelSize.y) * offset;//top left
		uv23.zw = uv + float2(texelSize.x, -texelSize.y) * offset;//bottom right

		half4 sum = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * 4;
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv01.xy);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv01.zw);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv23.xy);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv23.zw);
		return sum * 0.125;
	}
	
	
	v2f_UpSample Vert_UpSample(appdata v)
	{
		v2f_UpSample o;
		o.vertex = TransformObjectToHClip(v.vertex);
		o.texcoord = v.texcoord;
		
		// #if UNITY_UV_STARTS_AT_TOP
		// 	o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		// #endif
		float2 uv = o.texcoord;
		
		float4 texelSize = _MainTex_TexelSize * 0.5;
		float2 offset = float2(_Offset, _Offset);
		
		o.uv01.xy = uv + float2(-texelSize.x * 2, 0) * offset;
		o.uv01.zw = uv + float2(-texelSize.x, texelSize.y) * offset;
		o.uv23.xy = uv + float2(0, texelSize.y * 2) * offset;
		o.uv23.zw = uv + texelSize * offset;
		o.uv45.xy = uv + float2(texelSize.x * 2, 0) * offset;
		o.uv45.zw = uv + float2(texelSize.x, -texelSize.y) * offset;
		o.uv67.xy = uv + float2(0, -texelSize.y * 2) * offset;
		o.uv67.zw = uv - texelSize * offset;

		
		return o;
	}
	
	half4 Frag_UpSample(v2f_UpSample i): SV_Target
	{
		half4 sum = 0;
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.xy);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.zw) * 2;
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.xy);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.zw) * 2;
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.xy);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.zw) * 2;
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv67.xy);
		sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv67.zw) * 2;
		return sum * 0.0833;
	}
	
	ENDHLSL
	
	SubShader
	{
	    ZWrite Off ZTest Always
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex Vert
			#pragma fragment Frag_Filter
			
			ENDHLSL
			
		}

        Pass
		{
			
			HLSLPROGRAM
			#pragma vertex Vert_DownSample
			#pragma fragment Frag_DownSample
			
			ENDHLSL
			
		}

		Pass
		{
			
			Blend One One
			HLSLPROGRAM
			#pragma vertex Vert_UpSample
			#pragma fragment Frag_UpSample
			
			ENDHLSL
			
		}


        Pass
		{
			Blend One One
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag_ApplyBloom
			
			ENDHLSL
			
		}
	}
    
}

