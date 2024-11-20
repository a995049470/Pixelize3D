#ifndef PIXELIZE_SHADOW_INCLUDE
#define PIXELIZE_SHADOW_INCLUDE

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Assets/Res/Shader/Common/Dither.hlsl"
#include "Assets/Res/Shader/Common/PixelizeUniform.hlsl"


float3 _LightDirection;
float3 _LightPosition;
float4 _MainLightShadowmapTexture_TexelSize;

SamplerState my_point_clamp_sampler;
SamplerState my_bilinear_clamp_sampler;

float4 TransfromWorldToShadowHClip(float3 positionWS, float3 normalOS)
{
    float3 normalWS = TransformObjectToWorldNormal(normalOS);

#if _CASTING_PUNCTUAL_LIGHT_SHADOW
    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
#else
    float3 lightDirectionWS = _LightDirection;
#endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif

    return positionCS;
}


float4 TransfromObjectToShadowHClip(float3 positionOS, float3 normalOS)
{
    float3 positionWS = TransformObjectToWorld(positionOS);
    return TransfromWorldToShadowHClip(positionWS, normalOS);
}

float DitherShoadwAttenuation(float attenuation, float threshold)
{
    // float level = _ShadowPixelSize;
    // float current = floor(attenuation * level) / level;
    // float next = ceil(attenuation * level) / level;
    // float p = frac(attenuation * level);
    // attenuation = lerp(current, next, step(threshold, p));
   
    return attenuation;
}

//TODO:像素风格的阴影
float SampleDitherShadowmap(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 texelSize, float4 shadowCoord, ShadowSamplingData samplingData, half4 shadowParams, bool isPerspectiveProjection = true, float threshold = 0)
{
    // Compiler will optimize this branch away as long as isPerspectiveProjection is known at compile time
    if (isPerspectiveProjection)
        shadowCoord.xyz /= shadowCoord.w;

    float attenuation;
    float shadowStrength = shadowParams.x;

#ifdef _SHADOWS_SOFT
    if(shadowParams.y != 0)
    {
        attenuation = SampleShadowmapFiltered(TEXTURE2D_SHADOW_ARGS(ShadowMap, sampler_ShadowMap), shadowCoord, samplingData);
    }
    else
#endif
    {
        // float2 start = int2(shadowCoord.xy * texelSize.zw) / _ShadowPixelSize * _ShadowPixelSize * texelSize.xy;
        // attenuation = 0;
        // [unroll(25)]
        // for (int i = 0; i < _ShadowPixelSize * _ShadowPixelSize; i++) {
        //     float2 uv = shadowCoord.xy + float2(i / _ShadowPixelSize, i % _ShadowPixelSize) * texelSize.xy;
        //     float a = step(SAMPLE_TEXTURE2D(ShadowMap, my_point_clamp_sampler, uv).r, shadowCoord.z);
        //     attenuation += a;
        // }
        // attenuation /= (_ShadowPixelSize * _ShadowPixelSize);
        // 1-tap hardware comparison
        attenuation = float(SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz));
    }
    
    attenuation = DitherShoadwAttenuation(attenuation, threshold);

    attenuation = LerpWhiteTo(attenuation, shadowStrength);

    // Shadow coords that fall out of the light frustum volume must always return attenuation 1.0
    // TODO: We could use branch here to save some perf on some platforms.
    return BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : attenuation;
}

half MainLightRealtimeDitherShadow(float4 shadowCoord, float threshold)
{
    #if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
        return half(1.0);
    #elif defined(_MAIN_LIGHT_SHADOWS_SCREEN) && !defined(_SURFACE_TYPE_TRANSPARENT)
        return SampleScreenSpaceShadowmap(shadowCoord);
    #else
        ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
        half4 shadowParams = GetMainLightShadowParams();
        return SampleDitherShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), _MainLightShadowmapTexture_TexelSize, shadowCoord, shadowSamplingData, shadowParams, false, threshold);
    #endif
}


#endif