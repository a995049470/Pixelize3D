Shader "LPipeline/SampleLit"
{
    Properties
    {
        _AlbedoTex ("Albedo", 2D) = "white" {}
        _Albedo("Albedo", color) = (1, 1, 1, 1)
        _NormalTex ("Normal", 2D) = "bump" {}
        _MetallicTex("Metallic", 2D) = "white" {}
        _Metallic("Metallic", range(0, 1)) = 1
        _RoughnessTex("Roughness", 2D) = "white" {}
        _Roughness("Roughness", range(0, 1)) = 1
        _AOTex("AO", 2D) = "white" {}
        _AO("AO", range(0, 1)) = 1
        [HDR]_Emssion("Emssion", color) = (0, 0, 0, 0)
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
    }
    SubShader
    {
        //Gbuffer 
        pass
        {
            Name "GBuffer"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="GBuffer"
            }
            ZWrite On
            ZTest Less
            Cull [_Cull]
            
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            #include "GBuffer.hlsl"

            ENDHLSL
        }

        //输出黑色
        pass
        {
            Name "BlackOnly"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="BlackOnly"
            }
            ZWrite Off
            ZTest Equal
             Cull [_Cull]
            
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            #include "BlackOnly.hlsl"

            ENDHLSL
        }
        
        //产生shadowMap
        pass
        {
            Name "ShadowCaster"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="ShadowCaster"
            }
            ZWrite On
            ZTest Less
            ColorMask 0
            Cull Front
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            #include "ShadowCaster.hlsl"

            ENDHLSL
        }
    }
}
