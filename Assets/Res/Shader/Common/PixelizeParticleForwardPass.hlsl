#ifndef PIXELIZE_PARTICLE_FORWARD_PASS
#define PIXELIZE_PARTICLE_FORWARD_PASS

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_instancing
// #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
// #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
// #pragma multi_compile_fragment _ _SHADOWS_SOFT
// #pragma shader_feature_local _RECIVE_SHADOW
// #pragma shader_feature_local _ALPHA_CLIP

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/PixelizeLighting.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"

struct appdata
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float3 normalOS : NORMAL;
    float4 color : COLOR0;
};

struct v2f
{
    float4 position     : SV_POSITION;
    float2 uv           : TEXCOORD0;
    float3 normalWS     : NORMAL;
    float4 color        : COLOR;
    int2 originPos      : TEXCOORD1;
    float3 positionWS   : TEXCOORD3;

};

// CBUFFER_START(UnityPerMaterial)
// float4 _TintColor;
// float4 _OutlineColor;
// float3 _RelativePosition;
// float _Cutoff;
// CBUFFER_END

UNITY_INSTANCING_BUFFER_START(UnityInstancePerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _TintColor)
    UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
    UNITY_DEFINE_INSTANCED_PROP(float3, _RelativePosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
UNITY_INSTANCING_BUFFER_END(UnityInstancePerMaterial)

#define _TintColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _TintColor)
#define _OutlineColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _OutlineColor)
#define _RelativePosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _RelativePosition)
#define _Cutoff UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Cutoff)


Texture2D _MainTex;
SamplerState sampler_MainTex;


v2f vert(appdata v)
{
    v2f o = (v2f)0;
    o.uv = v.uv;
    AlignedPixelOutput result = TransformObjectToHClip_AlignedPixel(v.positionOS.xyz, GetObjectPosition());
    o.position = result.positionCS;
    o.normalWS = TransformObjectToWorldNormal(v.normalOS.xyz);
    o.color = v.color;
    o.positionWS = result.positionWS;
    return o;
}


float4 frag(v2f i) : SV_Target
{
    float4 col = _MainTex.Sample(sampler_MainTex, i.uv) * i.color;
    col *= _TintColor;
    DiscardPixel(int2(i.position.xy), 0);
    return col;
}



#endif