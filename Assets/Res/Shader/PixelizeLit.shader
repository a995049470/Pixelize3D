Shader "Lost/Pixelize Lit"
{
    Properties
    {
        [Main(Base)]
        _Group0("Base", float) = 1
        [Sub(Base)]
        _MainTex ("Texture", 2D) = "white" {}
        [Sub(Base)]//x y : flip  z : angle
        _MainTexRotate("MainTexRotate", vector) = (0, 0, 0, 0)
        [Sub(Base)]
        _Color ("Color", color) = (1, 1, 1, 1)
        [Sub(Base)]
        [HideInInspector]_RelativePosition("RelativePosition", vector) = (0, 0, 0, 0)
        [Sub(Base)]
        _HitColor("HitColor", color) = (0, 0, 0, 0)
        [Sub(Base)]
        _EmissionTex("EmissionColor", 2D) = "white" {}
        [Sub(Base)][HDR]
        _EmissionColor("EmissionColor", color) = (0, 0, 0, 0)
        [Main(BRDF, _LIGHTING)]
        _BRDFLight("Lighting", float) = 0
        [Sub(BRDF)]
        _Roughness("Roughness", Range(0, 1)) = 0
        //[Sub(BRDF)]
        //_Metallic("Metallic", Range(0, 1)) = 0
        //[Sub(BRDF)]
        //_Smoothness("Smoothness", Range(0, 8)) = 0
        [Sub(BRDF)]
        _SpecularIntensity("SpecularIntensity", Range(0, 128)) = 0
        [Sub(BRDF)]
        _DiffuseIntensity("DiffuseIntensity", Range(0, 1)) = 1
        [Sub(BRDF)]
        _AOMap("_AOMap", 2D) = "white" {}
        
        
        [Main(VitrualNormal, _VIRTUAL_MORMAL)]
        _VitrualNormal("VitrualNormal", float) = 0
        [Sub(VitrualNormal)]
        _NormalMap("NormalMap", 2D) = "bump" {}
        [Sub(VitrualNormal)]
        _NormalScale("NormalScale", Range(0, 8)) = 1
        [Sub(VitrualNormal)]
        _VirtualNormaLightDir("VirtualNormaLightDir", vector) = (0, 1, 0, 0)

        [Main(Outline)]
        _Outline("Outline", float) = 1
        [Sub(Outline)][HideInInspector]
        _OutlineColor ("OutlineColor(失效)", color) = (0, 0, 0, 1)
        [SubToggle(Outline)]
        _UseCustomOutlineV("UseCustomOutlineV", float) = 0
        [Sub(Outline)]
        _OutlineV("_OutlineV", Range(0, 1)) = 0.1
        
        [Main(Shadow, _RECIVE_SHADOW)]
        _ReciveShaodow("ReciveShadow", float) = 1
        
        [Main(AlphaClip, _ALPHA_CLIP)]
        _AlphaClip("AlphaClip", float) = 0
        [Sub(AlphaClip)]
        _Cutoff("Cutoff", Range(0, 1)) = 0.01

        [Main(Aligned, _CUSTOM_OBJECT_POSITION)]
        _UseCustomObjectPosition("UseCustomObjectPosition", float) = 0
        [Sub(Aligned)]
        _ObjectPosition("ObjectPosition", vector) = (0.71, 0.39, 7.19, 0)

        [Main(LightLevel, _REJECT_LIGHT_LEVEL)]
        _RejectLightLevel("RejectLightLevel", float) = 0
        
        [Main(HSV, _HSV_OFFSET)]
        _HSVOffset("HSV", float) = 0
        [Sub(HSV)]
        _H("H", Range(-1, 1)) = 0
        [Sub(HSV)]
        _S("S", Range(-1, 1)) = 0
        [Sub(HSV)]
        _V("V", Range(-1, 1)) = 0

        [Main(Wind, _WIND)]
        _Wind("Wind", float) = 0
        [Sub(Wind)]
        _PlantHeight("Height", float) = 1
        [Sub(Wind)]
        _Bend("Bend", Range(0, 6)) = 0
        [Sub(Wind)]
        _WindDir("WindDir(测试)", vector) = (1, 0, 0)
        [Sub(Wind)]
        _NoiseWindStrength("NoiseWindStrength(测试)", Range(-0.1, 0.1)) = 0 
        [Sub(Wind)]
        _BaseWindStrength("BaseWindStrength(测试)", Range(-0.1, 0.1)) = 0 
        [Sub(Wind)]
        _WindSpeed("WindSpeed(测试)", Range(0, 1)) = 0 
        [Sub(Wind)]
        _WindNoiseTile("WindNoiseTile(测试)", float) = 1
        [Sub(Wind)]
        _WindFPS("WindFPS(测试)", float) = 30

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
            Name "SRPDefaultUnlit"
            Tags
            {
                "LightMode"="SRPDefaultUnlit"
            }
            Blend One Zero
            ZWrite On
            ZTest Less
            Cull Off
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
            ZWrite On
            ZTest Less
            Cull Off
            
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
            Cull Off
            HLSLPROGRAM
            #include "Assets/Res/Shader/Common/PixelizeShadowCasterPass.hlsl"
            ENDHLSL
        }
        
    }
    CustomEditor "LWGUI.LWGUI"
}
