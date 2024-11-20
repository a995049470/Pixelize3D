// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Lost/Legacy Shaders/Particles/Alpha Blended Outline" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    [Toggle]_UseCustomObjectPosition("UseCustomObjectPosition", float) = 0
    _ObjectPosition("ObjectPosition", vector) = (0, 0, 0, 0)
}

Category {
    Tags 
    { 
        "Queue"="Geometry" 
        //"IgnoreProjector"="True" 
        "RenderType"="Opaque" 
        "PreviewType"="Plane" 
    }
    

    SubShader {
        Pass {
            Name "SRPDefaultUnlit"
            Tags
            {
                "LightMode"="SRPDefaultUnlit"
            }
            Blend One Zero
            ColorMask RGB
            ZWrite On
            ZTest Less
            Cull Off Lighting Off 
            HLSLPROGRAM
            #include "Assets\Res\Shader\BulitinEx\OpaqueParticlePass.hlsl"
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
            #define _OUTLINE 1
            #include "Assets\Res\Shader\BulitinEx\OpaqueParticlePass.hlsl"
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
            #include "Assets\Res\Shader\BulitinEx\OpaqueParticleShadowCasterPass.hlsl"
            ENDHLSL
        }
    }
}
}
