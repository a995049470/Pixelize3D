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
            #pragma multi_compile _ _PIXELATE

            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingFullscreen.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #define PIXELATE _PIXELATE

            TEXTURE2D_X(_SourceTex);
            SAMPLER(sampler_SourceTex);
        #if PIXELATE
            float4 _SourceTex_TexelSize;
            int _PixelSize;
            float _RenderScale;
        #endif



            

            half4 Fragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.uv;
                
            #if PIXELATE

                int2 pos = int2(uv * _SourceTex_TexelSize.zw);    
                pos = pos - pos % _PixelSize + _PixelSize / 2;
                half4 col = _SourceTex.Load(int3(pos, 0));
                

                //TODO:DownSample之后UpSample保持物体像素化的边缘稳定
                //尝试UpSample
                // float2 pxPerTx = 1.0f / _RenderScale;
                // float2 tx = uv * _SourceTex_TexelSize.zw;
                // float2 txOffset  = clamp(frac(tx) * pxPerTx, 0, 0.5) - clamp((1 - frac(tx)) * pxPerTx, 0, 0.5);
                // uv = floor(tx) + 0.5 + txOffset;
                // int2 min = int2(uv / _PixelSize + 0.5) * _PixelSize - _PixelSize / 2;
                // int2 max = int2(uv / _PixelSize + 0.5) * _PixelSize + _PixelSize / 2;
                // float2 t = (uv - min) / _PixelSize;
                // half4 colLD = _SourceTex.Load(int3(min.x, min.y, 0));
                // half4 colRD = _SourceTex.Load(int3(max.x, min.y, 0));
                // half4 colLU = _SourceTex.Load(int3(min.x, max.y, 0));
                // half4 colRU = _SourceTex.Load(int3(max.x, max.y, 0));
                // half4 col = lerp(
                //     lerp(colLD, colRD, t.x),
                //     lerp(colLU, colRU, t.x),
                //     t.y
                // );
                
                
                
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
