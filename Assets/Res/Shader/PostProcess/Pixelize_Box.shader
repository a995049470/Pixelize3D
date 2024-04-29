Shader "LPipeline/Pixelize Box"
{
    Properties
    {
        
    }
    SubShader
    {

        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }
        Pass
        {
            Blend One Zero
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "FullScreenTriangle.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            int _PixelSize;
            TEXTURE2D_X_FLOAT(_DepthTex);
            SAMPLER(sampler_DepthTex);
            

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.positionCS = TransformTriangleVertexToHClip(v.positionOS.xy);
                o.uv = TransformTriangleVertexToUV_FinalBlit(o.positionCS.xy);
                
                return o;
            }

            float4 SampeBox(float2 uv)
            {
                int2 pos = int2(uv * _MainTex_TexelSize.zw);    
                pos = pos - pos % _PixelSize;
                int count = _PixelSize * _PixelSize;
                float minDepth = 1;
                float4 color = 0;
                [unroll(64)]
                for (int i = 0; i < count; i++) {
                    int3 currentPos = int3(pos.xy + int2(i / _PixelSize, i % _PixelSize), 0);
                    float depth = _DepthTex.Load(currentPos).x;
                #if UNITY_REVERSED_Z
                    depth = 1 - depth;
                #endif
                    color = depth <= minDepth ? _MainTex.Load(currentPos) : color;
                    minDepth = depth <= minDepth ? depth : minDepth;
                }
                return color;
            }

            float4 SampeMid(float2 uv)
            {
                int2 pos = int2(uv * _MainTex_TexelSize.zw);    
                pos = pos - pos % _PixelSize + _PixelSize / 2;
                float4 color = _MainTex.Load(int3(pos, 0));
                return color;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 color = SampeMid(uv);
                
                return color;        
            }
            ENDHLSL
        }
    }
}
