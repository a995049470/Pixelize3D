Shader "LPipeline/Pixelize"
{
    Properties
    {
        [Toggle(_SMOOTH_UPSAMPLE)]_SmoothUpsample("SmoothUpsample", float) = 0
        [Toggle(_SAMPLE_PIXELIZE)]_SamplePixelize("SamplePixelize", float) = 0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "FullScreenTriangle.hlsl"
    #include "Assets/Res/Shader/PostProcess/FullScreenTriangle.hlsl"

    #define RENDER_TO_CAMERA_RENDER_TARGET _RENDER_TO_CAMERA_RENDER_TARGET


    #define SMOOTH_UPSAMPLE _SMOOTH_UPSAMPLE
    #define SAMPLE_PIXELIZE _SAMPLE_PIXELIZE
    
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
    float2 _CameraOffsetVS;
    float2 _CameraOffsetSS;
    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    
    TEXTURE2D_X(_DepthTex);
    

    v2f vert(appdata v)
    {
        v2f o = (v2f)0;
        o.position = v.positionOS;
        o.uv = TransformTriangleVertexToUV(v.positionOS.xy);
        return o;
    }
    
    float4 frag_pixelize(v2f i) : SV_Target
    {
        int2 pos = i.position.xy;

#if SAMPLE_PIXELIZE
        pos = pos - pos % _PixelSize + _PixelSize / 2;
        float4 col = _MainTex.Load(int3(pos, 0));
#else
        int3 start = int3(pos - pos % _PixelSize, 0);
    #if UNITY_REVERSED_Z
        float nearest = 65536;
    #else
        float nearest = -65536;
    #endif
        float4 col = 0;
        int count = _PixelSize * _PixelSize;
        [unroll(64)]
        for (int i = 0; i < count; i++) 
        {
            int3 curPos = start + int3(count % _PixelSize, count / _PixelSize, 0);
            float depth = _DepthTex.Load(curPos).r;
            float4 curColor = _MainTex.Load(curPos);
        #if UNITY_REVERSED_Z
            if(depth < nearest)
            {
                nearest = depth;
                col = curColor;
            }
        #else
            if(depth > nearest)
            {
                nearest = depth;
                col = curColor;
            }
        #endif
        }
#endif
        //float4 col = _MainTex.Load(int3(pos, 0));
        return col;
    }

    float4 frag_upsample(v2f i) : SV_TARGET
    {
        float2 uv = i.uv;
        
        //去除最外圈的像素
        uv = (uv - 0.5) * (_MainTex_TexelSize.zw - 2) / _MainTex_TexelSize.zw + 0.5;
    #if UNITY_UV_STARTS_AT_TOP
        uv += _CameraOffsetSS * float2(1, - 1);
    #else
        uv += _CameraOffsetSS;
    #endif
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
            #pragma shader_feature _SAMPLE_PIXELIZE
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
            #define _RENDER_TO_CAMERA_RENDER_TARGET 1
            #pragma shader_feature _SMOOTH_UPSAMPLE
            #pragma vertex vert
            #pragma fragment frag_upsample
            ENDHLSL
        }
    }
}
