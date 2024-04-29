#ifndef LPIPELINE_SHADOW_RECIVER_PASS
#define LPIPELINE_SHADOW_RECIVER_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

Texture2D _ShadowMap; SamplerState sampler_ShadowMap;
#define ShadowBias  0.0001
//TODO : PCSS 软阴影
float Visiable(float3 positionWS)
{
    float4 positionCS_Shadow = mul(_ShadowVP, float4(positionWS, 1.0));
    float2 uvSS_Shadow = ComputeScreenPos(positionCS_Shadow).xy;
    float depth_Shadow = _ShadowMap.Sample(sampler_ShadowMap, uvSS_Shadow);
    float base = 0.0f;
    float visiable = step(depth_Shadow, positionCS_Shadow.z + ShadowBias);
    visiable = visiable + (1 - visiable) * base;
    //超出shadermap边界的判断...  计算有点怪..
    float2 outside = step(0.4, abs(uvSS_Shadow - .5));
    visiable += outside.x + outside.y;
    visiable = saturate(visiable);
    return visiable;
}

#endif