#ifndef OPAQUE_PARTICLE_SHADOW_CASTER_PASS
#define OPAQUE_PARTICLE_SHADOW_CASTER_PASS

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
#pragma shader_feature_local _ALPHA_CLIP
#pragma multi_compile_instancing

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"

#define ALPHA_CLIP _ALPHA_CLIP

struct appdata
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
    float4 color : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 position : SV_POSITION;
    float4 positionCS : TEXCOORD1;
    float2 uv : TEXCOORD2;
    float4 color : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

// CBUFFER_START(UnityPerMaterial)
// float3 _RelativePosition;
// float _Cutoff;
// bool _UseFixedOriginPosition;
// CBUFFER_END

UNITY_INSTANCING_BUFFER_START(UnityInstancePerMaterial)
    //UNITY_DEFINE_INSTANCED_PROP(float3, _RelativePosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
    //UNITY_DEFINE_INSTANCED_PROP(bool, _UseFixedOriginPosition)
UNITY_INSTANCING_BUFFER_END(UnityInstancePerMaterial)

//#define _RelativePosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _RelativePosition)
#define _Cutoff UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Cutoff)
//#define _UseFixedOriginPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _UseFixedOriginPosition)

Texture2D _MainTex;
SamplerState sampler_MainTex;
float4 _TintColor;


v2f vert(appdata v)
{
    v2f o = (v2f)0;
    UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
    AlignedPixelOutput res = TransformObjectToHClip_AlignedPixel(v.positionOS.xyz, GetObjectPosition());
    o.positionCS = res.positionCS;
    o.position = TransfromWorldToShadowHClip(res.positionWS, v.normalOS);
    o.uv = v.texcoord;
    o.color = v.color * _TintColor;
    return o;
}


float4 frag(v2f i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    float4 col = 2.0f * i.color * _MainTex.Sample(sampler_MainTex, i.uv);
    col.a = saturate(col.a);
    Unity_Dither_Pixel(col.a, int2(i.position.xy), 1);
    return 0;
}

#endif