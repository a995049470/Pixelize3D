Shader "LPipeline/Light_Directional"
{
    Properties
    {
        [Toggle]_Diffuse("Diffuse", float) = 1
    }
    SubShader
    {
        //Gbuffer 
        pass
        {
            Name "Light_Directional"
            Tags
            {
                "RenderType"="Opaque"
            }
            ZWrite Off
            ZTest Always
            Blend One One
            Cull Back
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "Light_Directional.hlsl"

            ENDHLSL
        }

        
        
    }
}
