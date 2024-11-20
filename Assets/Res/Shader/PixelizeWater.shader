Shader "Lost/Pixelize Water"
{
    Properties
    {
        [Main(WaterColor)]
        _Group0("WaterColor", float) = 1
        [Sub(WaterColor)]
        _ShallowWaterColor("ShallowWaterColor", color) = (1, 1, 1, 1)
        [Sub(WaterColor)]
        _DeepWaterColor("DeepWaterColor", color) = (1, 1, 1, 1) 
        [Sub(WaterColor)]
        _ShallowWaterDepth("ShallowWaterDepth", float) = 0
        [Sub(WaterColor)]
        _DeepWaterDepth("DeepWaterDepth", float) = 0
        [Sub(WaterColor)]
        _WaterVisibility("WaterVisibility", Range(0, 1)) = 0.5
        
        [Main(Wave)]
        _Group1("Wave", float) = 1
        [Sub(Wave)]
        _WaveNoiseUVScaleX("WaveNoiseUVScaleX", float) = 1
        [Sub(Wave)]
        _WaveNoiseUVScaleY("WaveNoiseUVScaleY", float) = 1
        [Sub(Wave)]
        _WaveNoiseUVSpeedZ("WaveNoiseUVSpeedZ", float) = 1
        [Sub(Wave)]
        _WaveNoiseCutoff("WaveNoiseCutoff", Range(0, 1)) = 0
        [Sub(Wave)]
        _WaveColor("WaveColor", Color) = (1, 1, 1, 1)

        [Main(Outline)]
        _Outline("Outline", float) = 1
        [Sub(Outline)]
        _OutlineColor ("OutlineColor(失效)", color) = (0, 0, 0, 1)
        [Sub(Outline)]
        _OutlineV("_OutlineV", Range(0, 1)) = 0.1

        [Main(Aligned, _CUSTOM_OBJECT_POSITION)]
        _UseCustomObjectPosition("UseCustomObjectPosition", float) = 0
        [Sub(Aligned)]
        _ObjectPosition("ObjectPosition", vector) = (0.71, 0.39, 7.19, 0)

        // _WaveNoiseTex("WaveNoiseTex", 2D) = "white" {}
        // _WaveParam("WaveParam", vector) = (0, 0, 0, 0)
    }
    SubShader
    {

        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Transparent"
        }

        
        Pass
        {
            Tags
            {
                "LightMode"="UniversalForward"
            }

            Stencil
            {
                Ref 1
                ReadMask 1
                WriteMask 1
                Comp NotEqual
                Pass Keep
            }
            Blend One Zero
            ZWrite On
            ZTest LEqual
            Cull Back
            HLSLPROGRAM
            #include "Assets/Res/Shader/Common/PixelizeWaterPass.hlsl"
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
            #define _WATER_OUTLINE 1
            #include "Assets/Res/Shader/Common/PixelizeOutlinePass.hlsl"
            ENDHLSL
        }
    }
    CustomEditor "LWGUI.LWGUI"
}
