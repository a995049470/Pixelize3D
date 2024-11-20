#ifndef LPIPELINE_CUBE_SHADOW_CASTER_PASS
#define LPIPELINE_CUBE_SHADOW_CASTER_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
};

struct Varyings
{
    float4 positionCS : POSITION;
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
float4x4 _ShadowVP;

Varyings Vertex(Attributes i, uint instanceID : SV_InstanceID)
{
    CubeGBuffer data = _CubeGBuffer[instanceID];
    Varyings o = (Varyings)0;

    float4 positionWS = mul(data.localToWorldMatrix, i.positionOS);
    o.positionCS = mul(_ShadowVP, float4(positionWS.xyz, 1));
    
    return o;
}

float4 Fragment(Varyings i) : SV_TARGET
{
    return 0;
}


#endif