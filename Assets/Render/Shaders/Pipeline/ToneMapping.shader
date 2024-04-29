Shader "LPipeline/ToneMappint"
{
    Properties
    {
        [HideInInspector]_MainTex("MainTex", 2D) = "white" {}
        _AdaptedLum("AdaptedLum", range(0.1, 2)) = 1 
    }
    SubShader
    {
        
        
        pass
        {
            Name "ToneMapping"
            Tags
            {
                "RenderType"="Opaque"
                "LightMode"="ToneMapping"
            }
            ZWrite Off
            ZTest Always
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vertex
            #pragma fragment Fragment

            
            #include "ToneMapping.hlsl"

            ENDHLSL
        }
    }
}
