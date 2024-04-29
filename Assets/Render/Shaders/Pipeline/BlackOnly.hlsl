#ifndef LPIPELINE_BLACK_ONLY_PASS
#define LPIPELINE_BLACK_ONLY_PASS

//渲染BRDF基础色的地方

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : POSITION;
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Texture2D _AlbedoTex; SamplerState sampler_AlbedoTex;
Texture2D _AOTex; SamplerState sampler_AOTex;

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float3, _Albedo)
    UNITY_DEFINE_INSTANCED_PROP(float, _AO)
    UNITY_DEFINE_INSTANCED_PROP(float4, _Emssion)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)


Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
    o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
    o.uv = i.texcoord;
    return o;
}

#define LIGHT_AMBINE 0.00

//加点基色 不至于太黑
float4 Fragment(Varyings i) : SV_TARGET
{
    float4 emssion = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Emssion);
    return emssion;
    // UNITY_SETUP_INSTANCE_ID(i);
    // float3 Albedo = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Albedo);
    // float AO = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _AO);
    // float3 albedo = _AlbedoTex.Sample(sampler_AlbedoTex, i.uv).rgb * Albedo;
    // float ao = _AOTex.Sample(sampler_AOTex, i.uv).r * AO;
    // float3 color = LIGHT_AMBINE * ao * albedo;
    // return float4(color, 1) ;
}

#endif