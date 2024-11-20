Shader "LPipeline/Bloom2"
{
	Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _Threshold("Threshold", Range(0, 2)) = 1
        _SoftThreshold("SoftThreshold", Range(0, 1)) = 0
        _Intensity("Intensity", Range(0, 4)) = 1
        
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

    struct v2f
	{
		float4 vertex: SV_POSITION;
		float2 uv: TEXCOORD1;
	};
	
	

    v2f Vert(appdata v)
	{
		v2f o;
		o.vertex = TransformObjectToHClip(v.vertex);
		o.uv = v.texcoord;
		
		return o;
	}

    half4 Sample(float2 uv)
    {
        return _MainTex.Sample(sampler_MainTex, uv);
    }

    half4 SampleBox(float2 uv, float delta)
    {
        float4 o = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;
        half4 s = Sample(uv + o.xy) + Sample(uv + o.zy) +
        Sample(uv + o.xw) + Sample(uv + o.zw);
        return s * 0.25f;
    }

    half4 Prefilter(half4 c)
    {
        float brightness = max(c.r, max(c.g, c.b));
        half knee = _Threshold * _SoftThreshold;
        half soft = brightness - _Threshold + knee;
        soft = clamp(soft, 0, 2 * knee);
        soft = soft * soft / (4 * knee + 0.00001);
        half contribution = max(soft, brightness - _Threshold);
		contribution /= max(brightness, 0.00001);
        return c * contribution;
    }

    half4 Frag_Filter(v2f i) : SV_TARGET
    {
        return Prefilter(SampleBox(i.uv, 1));
    }
    
	
	half4 Frag_DownSample(v2f i): SV_Target
	{
		return SampleBox(i.uv, 1);
	}
	
	
	half4 Frag_UpSample(v2f i): SV_Target
	{
		return SampleBox(i.uv, 0.5);
	}
	
    half4 Frag_ApplyBloom(v2f i) : SV_TARGET
    {
        half4 color = SampleBox(i.uv, 0.5);
        color *= _Intensity;
        return color;
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
			#pragma vertex Vert
			#pragma fragment Frag_DownSample
			
			ENDHLSL
			
		}

		Pass
		{
			
			Blend One One
			HLSLPROGRAM
			#pragma vertex Vert
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

