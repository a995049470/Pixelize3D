Shader "LPipeline/Light_Point"
{
    Properties
    {
        _LightColor("_LightColor", color) = (0, 0, 0, 0)
        _LightParameter("_LightParameter", vector) = (0, 0, 0, 0)
        [Toggle]_Diffuse("Diffuse", float) = 1
        //_LightMask("LightMask", Cube) = "white" {}
    }
    SubShader
    {
        pass
        {
            Name "Light_Point"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="PointLight"
            }
            ZWrite Off
            //ZTest Less
            //Cull back
            ZTest Less
            Blend One One
            Cull front
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_instancing

            #include "Light_Point.hlsl"

            ENDHLSL
        }

        
        
        
    }
}
