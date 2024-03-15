Shader "LPipeline/DrawSkybox"
{
    Properties
    {
        _CubeMap("CubeMap", Cube) = "" {}
    }
    SubShader
    {
        
        pass
        {
            Tags 
            {
                "LightMode" = "DrawSkybox" 
            }
            ZWrite Off
            ZTest LEqual
            Cull Front

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment



            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD;
            };

            struct Varyings
            {
                float4 positionCS : POSITION;
                float3 dir : TEXCOORD0;
            };

            TextureCube _CubeMap; SamplerState sampler_CubeMap;
            

            Varyings Vertex(Attributes i)
            {
                Varyings o = (Varyings)0;
                o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
                //可能其他平台上不对...
                #if UNITY_REVERSED_Z
                    o.positionCS.z = 0;
                #else
                    o.positionCS.z = o.positionCS.w;
                #endif
                o.dir = normalize(i.positionOS.xyz);
                return o;
            }


            float4 Fragment(Varyings i) : SV_TARGET
            {
                float4 color = _CubeMap.Sample(sampler_CubeMap, i.dir);
                //return i.positionCS.z;
                return color;
            }
            ENDHLSL

        }
    }
}
