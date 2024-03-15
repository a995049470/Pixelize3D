#ifndef LPIPELINE_CUBE_GBUFFER_PASS
#define LPIPELINE_CUBE_GBUFFER_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
    float4 tangentOS : TANGENT;
    float3 normalOS : NORMAL;
};

struct Varyings
{
    float4 positionCS : POSITION;
    float2 uv : TEXCOORD0;
    float3 normalWS : TEXCOORD1;
    float3 albedo : TEXCOORD2;
    float metallic : TEXCOORD4;
    float roughness : TEXCOORD5;
    float ao : TEXCOORD6;
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
    o.uv = i.texcoord;
    
    float3 normalWS = mul((float3x3)data.localToWorldMatrix, i.normalOS);
    SafeNormalize(normalWS);
    float3 tangentWS = mul((float3x3)data.localToWorldMatrix, i.tangentOS);
    SafeNormalize(tangentWS);
    real sign = i.tangentOS.w * GetOddNegativeScale();
    float3 bitangentWS = cross(normalWS, tangentWS) * sign;
    float3x3 t2w = float3x3(tangentWS, bitangentWS, normalWS);
    float3 normalTS = data.normalTS * 2.0 - 1.0;
    o.normalWS = mul(normalTS, t2w);


    o.albedo = data.albedo.xyz;
    o.roughness = data.roughness;
    o.metallic = data.metallic;
    o.ao = data.ao;
    
    return o;
}

void Fragment(Varyings i, 
out float3 gbuffer0 : SV_TARGET0, 
out float3 gbuffer1 : SV_TARGET1, 
out float3 gbuffer2 : SV_TARGET2)
{
    float3 albedo = i.albedo;
    float3 normalWS = i.normalWS;
    float metallic = i.metallic;

    float roughness = i.roughness;
    float ao = i.ao;
    gbuffer0 = albedo;
    gbuffer1 = normalWS;
    gbuffer2 = float3(metallic, roughness, ao);
    
    //gbuffer2 = i.positionWS;
}


#endif