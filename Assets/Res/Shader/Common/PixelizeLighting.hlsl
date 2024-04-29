#ifndef PIXELIZE_LIGHTING_INCLUDE
#define PIXELIZE_LIGHTING_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Assets/Res/Shader/Common/Common.hlsl"
#include "Assets/Res/Shader/Common/Dither.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"

float _MinLDotN;
float _DitherWidth;

float3 DitherColor(float3 color, float levels, float threshold)
{   
    float brightness = (color.r + color.g + color.b) / 3;  
    float prev = floor(brightness * levels) / levels;
    float next = ceil(brightness * levels) / levels;
    float progress = frac(brightness * levels);
    progress = step(threshold, progress);
    color = color / max(0.01, brightness) * lerp(prev, next, progress);
    return color;
}

float3 LightingPixelLambert_LightColorDither(float3 baseColor, float3 lightColor, float3 lightDir, float3 normal, float threshold)
{
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    lightColor =  DitherColor(lightColor * LDotN, _LightLevel, threshold) * baseColor ;
    return lightColor;
}

float3 LightingPixelLambert_LDotNDither(float3 baseColor, float3 lightColor, float3 lightDir, float3 normal, float threshold)
{
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    LDotN01 *= 0.99f;
    float level = _LightLevel;
    float current = floor(LDotN01 * level) / level;
    float next = ceil(LDotN01 * level) / level;
    float p = frac(LDotN01 * level);
    LDotN01 = lerp(current, next, step(threshold, p));
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    lightColor = lightColor * LDotN * baseColor;
    return lightColor;
}

float3 LightingPixelLambert(float3 lightColor, float3 lightDir, float3 normal, float threshold)
{
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    return lightColor * LDotN;
}



float3 GetMainLightColor(float3 normal, float threshold)
{
    float3 finalColor = 0;
    //主光
    Light light = GetMainLight();
    float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
    finalColor.rgb += LightingPixelLambert(lightColor, light.direction, normal, threshold);
    return finalColor;
}

float3 GetMainLightColor(float3 normal, float4 shadowCoord, float threshold)
{
    float3 finalColor = 0;
    //主光
    Light light = GetMainLight();
    light.shadowAttenuation = MainLightRealtimeDitherShadow(shadowCoord, threshold);
    float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
    finalColor.rgb += LightingPixelLambert(lightColor, light.direction, normal, threshold);
    return finalColor;
}

float3 GetAdditionalLightColorSum(float3 normal, float3 positionWS, float threshold)
{
    float3 finalColor = 0;
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, positionWS);
        float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
        float3 dir = normalize(light.direction);
        finalColor.rgb += LightingPixelLambert(lightColor, dir, normal, threshold);
    }
    return finalColor;
}



float4 ApplyPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS)
{
    float3 finalLightColor = float3(0, 0, 0);
    float threshold = DitherThreshold(pos) * _DitherWidth;
    finalLightColor.rgb += GetMainLightColor(normal, threshold);
    finalLightColor.rgb += GetAdditionalLightColorSum(normal, positionWS, threshold);
    baseColor.rgb *= DitherColor(finalLightColor, _LightLevel, threshold);
    return baseColor;
}




float4 ApplyPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS, float4 shadowCoord)
{
    float3 finalLightColor = float3(0, 0, 0);
    float threshold = DitherThreshold(pos) * _DitherWidth;
    finalLightColor.rgb += GetMainLightColor(normal, shadowCoord, threshold);
    finalLightColor.rgb += GetAdditionalLightColorSum( normal, positionWS, threshold);
    baseColor.rgb *= DitherColor(finalLightColor, _LightLevel, threshold);
    return baseColor;
}



#endif