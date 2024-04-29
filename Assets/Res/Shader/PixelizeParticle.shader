Shader "Lost/Pixelize Particle"
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
        _TintColor ("Color", color) = (1, 1, 1, 1)


        // [Main(Outline)]
        // _Outline("Outline", float) = 1
        // [Sub(Outline)]
        // _OutlineColor ("OutlineColor", color) = (0, 0, 0, 1)
        
        
        // [Main(Shadow, _RECIVE_SHADOW)]
        // _ReciveShaodow("ReciveShadow", float) = 0
        
        // [Main(AlphaClip, _ALPHA_CLIP)]
        // _AlphaClip("AlphaClip", float) = 0
        // [Sub(AlphaClip)]
        // _Cutoff("Cutoff", Range(0, 1)) = 0.01

        
        [Main(DepthStencil)]
        _DepthStencil("DepthStencil", float) = 1
        [SubEnum(DepthStencil)] 
        _Addtive("float", float) = 0
        [SubEnum(DepthStencil, UnityEngine.Rendering.CullMode)] 
        _Cull ("Cull", Float) = 0
        [SubEnum(DepthStencil, UnityEngine.Rendering.BlendMode)] 
        _SrcBlend ("SrcBlend", Float) = 5
        [SubEnum(DepthStencil, UnityEngine.Rendering.BlendMode)] 
        _DstBlend ("DstBlend", Float) = 10
        [SubToggle(DepthStencil)] _ZWrite ("ZWrite ", Float) = 0
        [SubEnum(DepthStencil, UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4 // 4 is LEqual
        
    }
    SubShader
    {

        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Pass
        {
            Name "SRPDefaultUnlit"
            Tags
            {
                "LightMode"="SRPDefaultUnlit"
            }
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull [_Cull]
            HLSLPROGRAM
            #include "Assets/Res/Shader/Common/PixelizeParticleForwardPass.hlsl"
            ENDHLSL
        }

        
        
    }
    CustomEditor "LWGUI.LWGUI"
}
