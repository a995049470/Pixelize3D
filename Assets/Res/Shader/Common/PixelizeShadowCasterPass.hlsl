#ifndef PIXELIZE_SHADOW_CASTER_PASS
#define PIXELIZE_SHADOW_CASTER_PASS

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
#pragma shader_feature_local _ALPHA_CLIP
#pragma shader_feature_local _WIND
#pragma multi_compile_instancing

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/Wind.hlsl"

#define WIND _WIND
#define ALPHA_CLIP _ALPHA_CLIP

struct appdata
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 position : SV_POSITION;
    float4 positionCS : TEXCOORD1;
    float2 uv : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};



UNITY_INSTANCING_BUFFER_START(UnityInstancePerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
    UNITY_DEFINE_INSTANCED_PROP(float, _PlantHeight)
    UNITY_DEFINE_INSTANCED_PROP(float, _Bend)
    UNITY_DEFINE_INSTANCED_PROP(float3, _ObjectPosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _UseCustomObjectPosition)
UNITY_INSTANCING_BUFFER_END(UnityInstancePerMaterial)

#define _Cutoff UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Cutoff)
#define _PlantHeight UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _PlantHeight)
#define _Bend UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Bend)
#define _ObjectPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _ObjectPosition)
#define _UseCustomObjectPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _UseCustomObjectPosition)

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
    o.positionCS = result.positionCS;
    o.position = TransfromWorldToShadowHClip(result.positionWS, v.normalOS);
    o.uv = v.texcoord;
    return o;
}


float4 frag(v2f i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    float4 col = _MainTex.Sample(sampler_MainTex, i.uv);
#if ALPHA_CLIP
    clip(col.a - _Cutoff);
#endif
    return 0;
}

#endif