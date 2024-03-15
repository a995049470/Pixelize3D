#ifndef LPIPELINE_IBL_AMBIENT_PASS
#define LPIPELINE_IBL_AMBIENT_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Common.hlsl"
#include "BRDF.hlsl"
struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
  
};

struct Varyings
{
    float4 positionCS : POSITION;
    float4 positionSS : TEXCOORD0;
};

TextureCube _DiffuseEnvMap; SamplerState sampler_DiffuseEnvMap;
TextureCube _GlossyEnvMap; SamplerState sampler_GlossyEnvMap;
Texture2D _BRDFLut; SamplerState sampler_BRDFLut;
Texture2D _DepthTexture; SamplerState sampler_DepthTexture;
Texture2D _GBuffer0; SamplerState sampler_GBuffer0;
Texture2D _GBuffer1; SamplerState sampler_GBuffer1;
Texture2D _GBuffer2; SamplerState sampler_GBuffer2;
float4x4 _InvVP;
float _AmbientIntensity;

Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    o.positionCS = i.positionOS;
#if UNITY_UV_STARTS_AT_TOP
    o.positionCS.y = -o.positionCS.y;
#endif
    o.positionSS = ComputeScreenPos(o.positionCS);
    
    return o;
}

#define MAX_LOD_LEVEL 4
float4 Fragment(Varyings i) : SV_TARGET
{
    float2 uvSS = i.positionSS.xy / i.positionSS.w;
    float depth = _DepthTexture.Sample(sampler_DepthTexture, uvSS).r;
    float3 albedo = _GBuffer0.Sample(sampler_GBuffer0, uvSS).rgb;
    float3 normal = normalize(_GBuffer1.Sample(sampler_GBuffer1, uvSS).rgb);
    float3 positionWS = ScreenPosToWorldPosition(uvSS, depth, _InvVP);
    float3 viewDir = normalize(GetCameraPositionWS() - positionWS);
    float3 gbuffer2 = _GBuffer2.Sample(sampler_GBuffer2, uvSS);
    float metallic = gbuffer2.x;
    float roughness = gbuffer2.y;
    float ao = gbuffer2.z;
    float3 irradiance = _DiffuseEnvMap.Sample(sampler_DiffuseEnvMap, normal);

    float3 diffuse = irradiance * albedo;

    float NdotV = max(dot(normal, viewDir), 0);
    //NdotV = pow(1 - NdotV, 8) + NdotV;

    float3 reflect = 2.0 * dot(viewDir, normal) * normal - viewDir;
    float lod = roughness * MAX_LOD_LEVEL;
    float3 p0 = _GlossyEnvMap.SampleLevel(sampler_GlossyEnvMap, reflect, floor(lod));
    float3 p1 = _GlossyEnvMap.SampleLevel(sampler_GlossyEnvMap, reflect, ceil(lod));
    float3 prefilteredColor = lerp(p0, p1, frac(lod));
    // float3 prefilteredColor = _GlossyEnvMap.SampleLevel(sampler_GlossyEnvMap, reflect, lod);
    float3 f0 = 0.04f;
    f0 = lerp(f0, albedo, metallic); 
    float3 f = FresnelSchlickRoughness(NdotV, f0, roughness);
    float2 envBRDF = _BRDFLut.Sample(sampler_BRDFLut, float2(NdotV, roughness)).rg;
    float3 specular = prefilteredColor * (f * envBRDF.r + envBRDF.g);
    float3 ks = f;
    float3 kd = 1 - f;
    kd *= 1 - metallic;
    float3 ambient = kd * diffuse + specular * (step(dot(normal, normal), 0));
    
    return float4(ambient, 1);
}



#endif