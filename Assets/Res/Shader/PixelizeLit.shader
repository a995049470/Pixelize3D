Shader "Lost/Pixelize Lit"
{
    Properties
    {
        [Main(Base)]
        _Group0("Base", float) = 1
        [Sub(Base)]
        _MainTex ("Texture", 2D) = "white" {}
        [Sub(Base)]
        [HideInInspector]_RelativePosition("RelativePosition", vector) = (0, 0, 0, 0)
        [Sub(Base)]
        _Color ("Color", color) = (1, 1, 1, 1)
        
        
        [Main(Outline)]
        _Outline("Outline", float) = 1
        [Sub(Outline)]
        _OutlineColor ("OutlineColor", color) = (0, 0, 0, 1)
        
        
        [Main(Shadow, _RECIVE_SHADOW)]
        _ReciveShaodow("ReciveShadow", float) = 0
        
        [Main(AlphaClip, _ALPHA_CLIP)]
        _AlphaClip("AlphaClip", float) = 0
        [Sub(AlphaClip)]
        _Cutoff("Cutoff", Range(0, 1)) = 0.01
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
            Name "PixelizeLit"
            Tags
            {
                "LightMode"="SRPDefaultUnlit"
            }
            Blend One Zero
            ZWrite On
            ZTest Less
            Cull Back
            HLSLPROGRAM
            #include "Assets/Res/Shader/Common/PixelizeLitForwardPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "PixelOutline"
            Tags
            {
                "LightMode"="PixelOutline"
            }
            Blend One Zero
            ZWrite Off
            ZTest Equal
            Cull Back
            HLSLPROGRAM
            #include "Assets/Res/Shader/Common/PixelizeOutlinePass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode"="ShadowCaster"
            }
            Blend One Zero
            ZWrite On
            ZTest Less
            Cull Back
            HLSLPROGRAM
            #include "Assets/Res/Shader/Common/PixelizeShadowCasterPass.hlsl"
            ENDHLSL
        }
        
    }
    CustomEditor "LWGUI.LWGUI"
}
