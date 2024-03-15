Shader "LPipeline/Pixelize"
{
    Properties
    {
        [Toggle(_SMOOTH_UPSAMPLE)]_SmoothUpsample("SmoothUpsample", float) = 0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "FullScreenTriangle.hlsl"

    #define RENDER_TO_CAMERA_RENDER_TARGET _RENDER_TO_CAMERA_RENDER_TARGET

    #define UV_AT_TOP RENDER_TO_CAMERA_RENDER_TARGET && !UNITY_UV_STARTS_AT_TOP

    #define SMOOTH_UPSAMPLE _SMOOTH_UPSAMPLE
    
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

    int _PixelSize;
    float _RenderScale;
    float4 _MainTex_TexelSize;
    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    
    
    

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
    
    float4 frag_pixelize(v2f i) : SV_Target
    {
        int2 pos = i.position.xy;
        pos = pos - pos % _PixelSize + _PixelSize / 2;
        float4 col = _MainTex.Load(int3(pos, 0));
        return col;
    }

    float4 frag_upsample(v2f i) : SV_TARGET
    {
        float2 uv = i.uv;
    #if SMOOTH_UPSAMPLE
        float2 pxPerTx = 1.0f / _RenderScale;
        float2 tx = uv * _MainTex_TexelSize.zw;
        float2 txOffset = clamp(frac(tx) * pxPerTx, 0, 0.5) - clamp((1 - frac(tx)) * pxPerTx, 0, 0.5);
        uv = (floor(tx) + 0.5 + txOffset) * _MainTex_TexelSize.xy;
    #endif
        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
        return col;
    }

    ENDHLSL

    SubShader
    {

        Pass
        {
            Name "Pixelize"
            Blend One Zero
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag_pixelize
            ENDHLSL
        }

        Pass
        {
            Name "Upsample"
            Blend One Zero
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #define _RENDER_TO_CAMERA_RENDER_TARGET
            #pragma shader_feature _SMOOTH_UPSAMPLE
            #pragma vertex vert
            #pragma fragment frag_upsample
            ENDHLSL
        }
    }
}
