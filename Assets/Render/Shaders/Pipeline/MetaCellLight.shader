Shader "LPipeline/MetaCellLight"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white"

    }
    SubShader
    {
        //产生shadowMap
        pass
        {
            Name "MetaCellLit"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="MetaCellLit"
            }
            ZWrite On
            ZTest Less
            Cull Off
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment
            //#pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD;
            };

            struct Varyings
            {
                float4 positionCS : POSITION;
                float3 cellLightUV : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            float3 _Origin;
            float3 _BlockNum;
            float3 _BlockSize;
            Texture3D _GlobalLightColorTexture;
            SamplerState sampler_GlobalLightColorTexture;
            Texture2D _MainTex;
            SamplerState sampler_MainTex;
            
            Varyings Vertex(Attributes i)
            {
                Varyings o = (Varyings)0;
                float3 positionWS = TransformObjectToWorld(i.positionOS);
                float3 cellLightUV = (positionWS - _Origin) / (_BlockNum * _BlockSize);
                o.positionWS = positionWS;
                o.cellLightUV = cellLightUV;
                o.positionCS = TransformObjectToHClip(i.positionOS);
                o.uv = i.texcoord;
                return o;
            }

            float4 Fragment(Varyings i) : SV_TARGET
            {
                //return float4(uv, 1);
                float3 lightColor = _GlobalLightColorTexture.Sample(sampler_GlobalLightColorTexture, i.cellLightUV).xyz;
                //return i.cellLightUV.xyzx;
                //lightColor = step(0, i.cellLightUV - .5);
                //return i.positionWS.xyzx;
                float3 color = _MainTex.Sample(sampler_MainTex, i.uv);
                lightColor /= (1 + lightColor);
                color *= lightColor;
                return float4(color, 1);
            }
            ENDHLSL
        }
    }
}
