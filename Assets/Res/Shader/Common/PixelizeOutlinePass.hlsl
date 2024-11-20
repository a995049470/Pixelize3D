#ifndef PIXELIZE_OUTLINE_PASS
#define PIXELIZE_OUTLINE_PASS


#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_instancing
#pragma shader_feature_local _WIND
#pragma shader_feature_local _ALPHA_CLIP

//#pragma multi_compile_fragment _ _PIXELIZE_BRDF
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/PixelizeLighting.hlsl"
#include "Assets/Res/Shader/Common/Wind.hlsl"

#define WIND _WIND
#define BRDF _PIXELIZE_BRDF
#define ALPHA_CLIP _ALPHA_CLIP
#define WATER_OUTLINE _WATER_OUTLINE


struct appdata
{
    float4 positionOS           : POSITION;
    float3 normalOS             : NORMAL;
    float2 texcoord             : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 position             : SV_POSITION;
    float originPos             : TEXCOORD1;
    float3 positionWS           : TEXCOORD2;
    float3 normalWS             : TEXCOORD3;
    float2 uv                   : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};


UNITY_INSTANCING_BUFFER_START(UnityInstancePerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
    UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
    UNITY_DEFINE_INSTANCED_PROP(float3, _RelativePosition)
    UNITY_DEFINE_INSTANCED_PROP(float4, _HitColor)
    UNITY_DEFINE_INSTANCED_PROP(float, _Roughness)
    UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
    UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
    UNITY_DEFINE_INSTANCED_PROP(float3, _ObjectPosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _UseCustomObjectPosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _Outline)
    UNITY_DEFINE_INSTANCED_PROP(float, _PlantHeight)
    UNITY_DEFINE_INSTANCED_PROP(float, _Bend)
    UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
    UNITY_DEFINE_INSTANCED_PROP(float, _OutlineV)
    UNITY_DEFINE_INSTANCED_PROP(float, _UseCustomOutlineV)
UNITY_INSTANCING_BUFFER_END(UnityInstancePerMaterial)



#define _Color UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Color)
#define _OutlineColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _OutlineColor)
#define _RelativePosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _RelativePosition)
#define _HitColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _HitColor)
#define _Roughness UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Roughness)
#define _Metallic UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Metallic)
#define _Smoothness UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Smoothness)
#define _ObjectPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _ObjectPosition)
#define _UseCustomObjectPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _UseCustomObjectPosition)
#define _Outline UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Outline)
#define _PlantHeight UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _PlantHeight)
#define _Bend UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Bend)
#define _Cutoff UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Cutoff)
#define _OutlineV UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _OutlineV)
#define _UseCustomOutlineV UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _UseCustomOutlineV)

#define SCALE_V 0.03

Texture2D _MainTex;
SamplerState sampler_MainTex;


v2f vert(appdata v)
{
    v2f o = (v2f)0;
    UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
    float3 objectPositon = lerp(GetObjectPosition(), _ObjectPosition, _UseCustomObjectPosition);
    float3 positionWS = TransformObjectToWorld(v.positionOS.xyz);
#if WIND
    float height = _PlantHeight;
    float bend = _Bend;
    positionWS = VertOffsetByWind(positionWS, 0, height, bend, GetObjectPosition());
#endif
    AlignedPixelOutput result = TransformWorldToHClip_AlignedPixel(positionWS, objectPositon);
    float outline = step(0.5, _Outline);
    o.position = float4(-2, -2, 0, 1) * (1 - outline) + result.positionCS * outline;

    float3 originPosWS = result.originPosWS;
    o.originPos = (originPosWS.x + 0.177) * 17.176 + originPosWS.y * 123.131 - originPosWS.z * 319.7931 + 0.2137;
    
    o.uv = v.texcoord;
    o.normalWS = TransformObjectToWorldNormal(v.normalOS);
    o.positionWS = result.positionWS;

    return o;
}

int RGBToInt(float4 color)
{
    int r = uint(color.r * 255);
    int g = uint(color.g * 255);
    int b = uint(color.b * 255);
    return r | (g << 8) | (b << 16);
}

float4 frag(v2f i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    DiscardPixel(int2(i.position.xy), 0);
    float4 col = _MainTex.Sample(sampler_MainTex, i.uv) * _Color;
#if ALPHA_CLIP
    clip(col.a - _Cutoff);
#endif

#if WATER_OUTLINE
    float3 outlineColor = float3(0, 0, 0);
#else
    float3 outlineColor = ApplyPixelizeLighting(col, float3(0, 1, 0), i.position.xy, i.positionWS, 0, float2(1, 0), 1);
#endif
    //outlineColor += _HitColor.rgb;
    float3 hsv = RgbToHsv(outlineColor);
    float scale_v = lerp(SCALE_V, SCALE_V, _UseCustomOutlineV);
    hsv.z *= scale_v;
    outlineColor = HsvToRgb(hsv);
    float4 color = float4(outlineColor, i.originPos);
    color.rbg += _HitColor.rgb;
    return color;
}

#endif