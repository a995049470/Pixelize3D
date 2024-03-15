Shader "LPipeline/Pixelize Outline"
{
    Properties
    {
        
        [Main(InsideOutline, _INSIDE_OUTLINE)]
        _InsideOutline("InsideOutline", float) = 0
        [Sub(InsideOutline)]
        _MinDepthOffset("MinDepthOffset", Range(0, 1)) = 0.5
    }
    SubShader
    {

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ _INSIDE_OUTLINE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/Res/Shader/Common/AlignedPixel.hlsl"

            #define INSIDE_OUTLINE _INSIDE_OUTLINE

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

            
            Texture2D<float4> _OutlineTex;
            float4 _OutlineTex_TexelSize;
            float _MinDepthOffset;
            

            TEXTURE2D_X_FLOAT(_DepthTex);
            SAMPLER(sampler_DepthTex);

            

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.position = v.positionOS;
                o.uv = v.uv;
            #if UNITY_UV_STARTS_AT_TOP
                o.uv.y = 1.0 - o.uv.y;
            #endif
                return o;
            }
            

            float OrthoLinearEyeDepth(float depth)
            {
            #if UNITY_REVERSED_Z
                depth = 1 - depth;
            #endif
                return lerp(_ProjectionParams.y, _ProjectionParams.z, depth);
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 color = 0;
                
                int2 midPos = int2(i.position.xy);
                // midoutline == w
                float4 midOutline = _OutlineTex.Load(int3(midPos, 0));
                DiscardPixel(midPos, 0);
                //if(midOutline.w == 0) discard;
            #if INSIDE_OUTLINE
                float midDepth = OrthoLinearEyeDepth(_DepthTex.Load(int3(midPos, 0)).x);
                for (int i = 0; i < 9; i++) {
                    int x = (i % 3 - 1) * _PixelSize;
                    int y = (i / 3 - 1) * _PixelSize;
                    int2 pos = midPos + int2(x, y);
                    float4 outline = _OutlineTex.Load(int3(pos, 0));
                    float depth = OrthoLinearEyeDepth(_DepthTex.Load(int3(pos, 0)).x);
                    if((outline.w != 0) && ((outline.w != midOutline.w && depth < midDepth) || (midDepth - depth > _MinDepthOffset)))
                    {
                        midDepth = depth;
                        color.rgb = outline.rgb;
                        color.a = 1;
                    }
                }
            #else
                float midDepth = _DepthTex.Load(int3(midPos, 0)).x;
                for (int i = 0; i < 9; i++) {
                    int x = (i % 3 - 1) * _PixelSize;
                    int y = (i / 3 - 1) * _PixelSize;
                    int2 pos = midPos + int2(x, y);
                    float4 outline = _OutlineTex.Load(int3(pos, 0));
                    float depth = _DepthTex.Load(int3(pos, 0)).x;
                    if((outline.w != midOutline.w && depth < midDepth))
                    {
                        midDepth = depth;
                        color.rgb = outline.rgb;
                        color.a = 1;
                    }
                }
            #endif
                return color;
            }
            ENDHLSL
        }
    }
    CustomEditor "LWGUI.LWGUI"
}
