Shader "LPipeline/Colorful"
{
    Properties
    {
        _Frequency("频率", float) = 1
        _Offset("偏移", float) = 0
        _Alpha("强度", Range(0, 1)) = 0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    
    #include "FullScreenTriangle.hlsl"

    #define RENDER_TO_CAMERA_RENDER_TARGET _RENDER_TO_CAMERA_RENDER_TARGET

    #define UV_AT_TOP RENDER_TO_CAMERA_RENDER_TARGET && !UNITY_UV_STARTS_AT_TOP
    
    struct appdata
    {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f
    {
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    float _Frequency;
    float _Alpha;
    float _Offset;

    v2f vert(appdata v)
    {
        v2f o = (v2f)0;
        o.position = v.positionOS;
        o.uv = v.uv;
        #if UV_AT_TOP
            o.uv.y = 1.0 - o.uv.y;
        #endif
        return o;
    }

    float3 RgbToHsv(float3 c)
    {
        const float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
        float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
        float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
        float d = q.x - min(q.w, q.y);
        const float e = 1.0e-4;
        return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
    }

    float3 HsvToRgb(float3 c)
    {
        const float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
        float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
        return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
    }
    



    float4 frag_colorful(v2f i) : SV_Target
    {
        float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).rgb;
        color = RgbToHsv(color);
        color = 1.0 - color;
        color = sin(color * PI * _Frequency + _Offset);
        color = HsvToRgb(color);
        return float4(color, 1);
    }

    float4 frag_combine(v2f i) : SV_TARGET
    {
        float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).rgb;
        return float4(color * _Alpha, 1);
    }

    ENDHLSL

    SubShader
    {

        Pass
        {
            Name "Colorful"
            Blend One Zero
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag_colorful
            ENDHLSL
        }

        Pass
        {
            Name "Combine"
            Blend One One
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #define _RENDER_TO_CAMERA_RENDER_TARGET
            #pragma vertex vert
            #pragma fragment frag_combine
            ENDHLSL
        }
    }
}
