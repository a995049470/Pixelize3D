#ifndef PIXELIZE_LIGHTING_INCLUDE
#define PIXELIZE_LIGHTING_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Assets/Res/Shader/Common/Common.hlsl"
#include "Assets/Res/Shader/Common/Dither.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"

float _MinLDotN;
float _DitherWidth;


float3 LightingPixelLambert(float3 lightColor, float3 lightDir, float3 normal, float threshold)
{
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    LDotN01 *= 0.99f;
    float level = _LightLevel;
    float current = floor(LDotN01 * level) / level;
    float next = ceil(LDotN01 * level) / level;
    float p = frac(LDotN01 * level);
    LDotN01 = lerp(current, next, step(threshold * _DitherWidth, p));
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    return lightColor * LDotN;
}

float3 ApplyMainLight(float4 baseColor, float3 normal, float threshold)
{
    float3 finalColor = 0;
    //主光
    Light light = GetMainLight();
    float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
    finalColor.rgb += baseColor.rgb * LightingPixelLambert(lightColor, light.direction, normal, threshold);
    return finalColor;
}

float3 ApplyMainLight(float4 baseColor, float3 normal, float4 shadowCoord, float threshold)
{
    float3 finalColor = 0;
    //主光
    Light light = GetMainLight();
    light.shadowAttenuation = MainLightRealtimeDitherShadow(shadowCoord, threshold);
    float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
    finalColor.rgb += baseColor.rgb * LightingPixelLambert(lightColor, light.direction, normal, threshold);
    return finalColor;
}

float3 ApplyAdditionalLights(float3 baseColor, float3 normal, float3 positionWS, float threshold)
{
    float3 finalColor = 0;
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, positionWS);
        float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
        finalColor.rgb += baseColor.rgb * LightingPixelLambert(lightColor, light.direction, normal, threshold);
    }
    return finalColor;
}



float4 ApplyPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS)
{
    float4 finalColor = float4(0, 0, 0, baseColor.a);
    float threshold = DitherThreshold(pos);
    finalColor.rgb += ApplyMainLight(baseColor, normal, threshold);
    finalColor.rgb += ApplyAdditionalLights(baseColor, normal, positionWS, threshold);
    return finalColor;
}




float4 ApplyPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS, float4 shadowCoord)
{
    float4 finalColor = float4(0, 0, 0, baseColor.a);
    float threshold = DitherThreshold(pos);
    finalColor.rgb += ApplyMainLight(baseColor, normal, shadowCoord, threshold);
    finalColor.rgb += ApplyAdditionalLights(baseColor, normal, positionWS, threshold);
    return finalColor;
}



#endif