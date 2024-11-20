Shader "Hidden/Universal Render Pipeline/Blit"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100

        Pass
        {
            Name "Blit"
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex FullscreenVert
            #pragma fragment Fragment
            #pragma multi_compile_fragment _ _LINEAR_TO_SRGB_CONVERSION
            #pragma multi_compile _ _USE_DRAW_PROCEDURAL
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            #pragma multi_compile _ _SMOOTH_UPSAMPLE
            #pragma multi_compile _ _PIXELIZE_DISTORTION

            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingFullscreen.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Assets\Res\Shader\Common\PixelizeDistortion.hlsl"
            #define SMOOTH_UPSAMPLE _SMOOTH_UPSAMPLE
            #define PIXELIZE_DISTORTION _PIXELIZE_DISTORTION

            SamplerState sampler_PointClamp;
            SamplerState sampler_LinearClamp;
            SamplerState sampler_PointRepeat;
            SamplerState sampler_LinearRepeat;

            TEXTURE2D_X(_SourceTex);
            SAMPLER(sampler_SourceTex);
    
        #if SMOOTH_UPSAMPLE
            float4 _SourceTex_TexelSize;
            int _PixelSize;
            float _RenderScale;
        #endif



            

            half4 Fragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.uv;
                uv = GetDistortionUV(uv);
            #if 0
                float2 pxPerTx = 1.0f / _RenderScale;
                float2 tx = uv * _SourceTex_TexelSize.zw;
                float2 txOffset = clamp(frac(tx) * pxPerTx, 0, 0.5) - clamp((1 - frac(tx)) * pxPerTx, 0, 0.5);
                uv = (floor(tx) + 0.5 + txOffset) * _SourceTex_TexelSize.xy;
                half4 col = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, uv);
            #else   
                half4 col = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, uv);
            #endif
            
    
                #ifdef _LINEAR_TO_SRGB_CONVERSION
                col = LinearToSRGB(col);
                #endif

                #if defined(DEBUG_DISPLAY)
                half4 debugColor = 0;

                if(CanDebugOverrideOutputColor(col, uv, debugColor))
                {
                    return debugColor;
                }
                #endif

                return col;
            }
            ENDHLSL
        }
    }
}
