Shader "LPipeline/IBLGlossyBlur"
{
    Properties
    {
        [HideInInspector]_MainTex("MainTex", Cube) = "" {}
        [HideInInspector]_Roughness("Roughness", float) = 0
        
    }
    SubShader
    {
        
        
        pass
        {
            Name "IBLGlossyBlur"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="IBLGlossyBlur"
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

            #include "IBLGlossyBlur.hlsl"
            ENDHLSL
        }
    }
}
