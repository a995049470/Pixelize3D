#ifndef LPIPELINE_LIGHT_DIRECTIONAL_PASS
#define LPIPELINE_LIGHT_DIRECTIONAL_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#include "Common.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
};

struct Varyings
{
    float4 positionCS : POSITION;
    //屏幕空间
    float4 positionSS : TEXCOORD0;
};

Texture2D _DepthTexture; SamplerState sampler_DepthTexture;
Texture2D _GBuffer0; SamplerState sampler_GBuffer0;
Texture2D _GBuffer1; SamplerState sampler_GBuffer1;
Texture2D _GBuffer2; SamplerState sampler_GBuffer2;


float4 _LightColor;
float3 _LightDirection;
float4x4 _InvVP;
float4x4 _ShadowVP;
float _Diffuse;

#include "ShadowReciver.hlsl"
#include "BRDF.hlsl"

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


float4 Fragment(Varyings i) : SV_TARGET
{
    float2 uvSS = i.positionSS.xy;
    float3 albedo = _GBuffer0.Sample(sampler_GBuffer0, uvSS).rgb;
    float3 normalWS = normalize(_GBuffer1.Sample(sampler_GBuffer1, uvSS).rgb);
    float3 lightDir = _LightDirection;
    float ldotn = dot(normalWS, lightDir);
    ldotn = max(0, ldotn);

    float depth = _DepthTexture.Sample(sampler_DepthTexture, uvSS).r;
    float3 positionWS = ScreenPosToWorldPosition(uvSS, depth, _InvVP);
    float visiable = Visiable(positionWS);
    float3 viewDir = normalize(GetCameraPositionWS() - positionWS);
    float3 gbuffer2 = _GBuffer2.Sample(sampler_GBuffer2, uvSS);
    float metallic = gbuffer2.x;
    float roughness = gbuffer2.y;
    float ao = gbuffer2.z;
    float3 lightColor = _LightColor;
    float2 strength = float2(_Diffuse, 1);
    float3 brdf = BDRF(lightDir, viewDir, normalWS, albedo, lightColor, roughness, metallic, ao, strength);

    float3 color = brdf;
    //color *= visiable;
    
    
    return float4(color, 1);
}


#endif