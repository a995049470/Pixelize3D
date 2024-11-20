Shader "LPipeline/MetaGIDiffuse"
{
    Properties
    {
        
    }
    SubShader
    {
        pass
        {
            Name "MetaCellGIDiffuse"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="MetaCellGIDiffuse"
            }
            ZWrite Off
            ZTest Always
            Blend One One
            Cull Off
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            #include "MetaGIDiffuse.hlsl"

            ENDHLSL
        }

        
        
        
    }
}
