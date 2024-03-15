Shader "LPipeline/BRDFPercompute"
{
    Properties
    {

    }
    SubShader
    {
        
        
        pass
        {
            Name "IBLAmbient"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="IBLBlur"
            }
            ZWrite Off
            ZTest Always
            Cull Off
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "BRDFPercompute.hlsl"
            ENDHLSL
        }
    }
}