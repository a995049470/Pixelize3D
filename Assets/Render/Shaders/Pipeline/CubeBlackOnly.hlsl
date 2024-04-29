#ifndef LPIPELINE_CUBE_BLACK_ONLY_PASS
#define LPIPELINE_CUBE_BLACK_ONLY_PASS

//渲染BRDF基础色的地方

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;

};

struct Varyings
{
    float4 positionCS : POSITION;
    float2 uv : TEXCOORD0;

};

struct CubeGBuffer
{
    float4x4 localToWorldMatrix;
    float3 albedo;
    float metallic;
    float3 normalTS;
    float roughness;
    float ao;
};

StructuredBuffer<CubeGBuffer> _CubeGBuffer;

Varyings Vertex(Attributes i, uint instanceID : SV_InstanceID)
{
    CubeGBuffer data = _CubeGBuffer[instanceID];
    Varyings o = (Varyings)0;
    float4 positionWS = mul(data.localToWorldMatrix, i.positionOS);
    o.positionCS = TransformWorldToHClip(positionWS.xyz);
    //o.uv = i.texcoord;
    
    return o;
}
#define LIGHT_AMBINE 0.03

//加点基色 不至于太黑
float4 Fragment(Varyings i) : SV_TARGET
{
    return 0;
    // UNITY_SETUP_INSTANCE_ID(i);
    // float3 Albedo = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Albedo);
    // float AO = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _AO);
    // float3 albedo = _AlbedoTex.Sample(sampler_AlbedoTex, i.uv).rgb * Albedo;
    // float ao = _AOTex.Sample(sampler_AOTex, i.uv).r * AO;
    // float3 color = LIGHT_AMBINE * ao * albedo;
    // return float4(color, 1) ;
}

#endif