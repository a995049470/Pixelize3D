#ifndef PIXELIZE_LIGHTING_INCLUDE
#define PIXELIZE_LIGHTING_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Assets/Res/Shader/Common/Common.hlsl"
#include "Assets/Res/Shader/Common/Dither.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"
#include "Assets/Render/Shaders/Pipeline/BRDF.hlsl"
#include "Assets/Res/Shader/Common/Temp.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"

float _MinLDotN;
float _DitherWidth;
float _LightLevel;
float4 _PixelLightingParameter;
float4 _PixelAmbientLight;

#define DiffuseIntensity _PixelLightingParameter.x
#define SpecularIntensity _PixelLightingParameter.y
#define DitherPower _PixelLightingParameter.z

// #define DiffuseIntensity 1
// #define SpecularIntensity 1
// #define DitherPower 1

float GetLightLevel()
{
    float level = _LightLevel;
#if REJECT_LIGHT_LEVEL
    level = 65536;
#endif
    return level;
}

//TODO:灯光移动时候边缘在闪烁。。。
Light GetAdditionalPixelizePerObjectLight(int perObjectLightIndex, float3 positionWS)
{
    // Abstraction over Light input constants
#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
    float4 lightPositionWS = _AdditionalLightsBuffer[perObjectLightIndex].position;
    half3 color = _AdditionalLightsBuffer[perObjectLightIndex].color.rgb;
    half4 distanceAndSpotAttenuation = _AdditionalLightsBuffer[perObjectLightIndex].attenuation;
    half4 spotDirection = _AdditionalLightsBuffer[perObjectLightIndex].spotDirection;
#ifdef _LIGHT_LAYERS
    uint lightLayerMask = _AdditionalLightsBuffer[perObjectLightIndex].layerMask;
#else
    uint lightLayerMask = DEFAULT_LIGHT_LAYERS;
#endif

#else   

    float4 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex];
    //lightPositionWS.xyz = PositionWSAlignedPixel(lightPositionWS.xyz);
    half3 color = _AdditionalLightsColor[perObjectLightIndex].rgb;
    half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];
    half4 spotDirection = _AdditionalLightsSpotDir[perObjectLightIndex];
#ifdef _LIGHT_LAYERS
    uint lightLayerMask = asuint(_AdditionalLightsLayerMasks[perObjectLightIndex]);
#else
    uint lightLayerMask = DEFAULT_LIGHT_LAYERS;
#endif

#endif

    // Directional lights store direction in lightPosition.xyz and have .w set to 0.0.
    // This way the following code will work for both directional and punctual lights.
    float3 lightVector = lightPositionWS.xyz - positionWS * lightPositionWS.w;
    float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);

    half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));
    // full-float precision required on some platforms
    float attenuation = half(DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy) * AngleAttenuation(spotDirection.xyz, lightDirection, distanceAndSpotAttenuation.zw));

    Light light;
    light.direction = lightDirection;
    light.distanceAttenuation = min(1.0, attenuation);
    light.shadowAttenuation = 1.0; // This value can later be overridden in GetAdditionalLight(uint i, float3 positionWS, half4 shadowMask)
    light.color = color;
    light.layerMask = lightLayerMask;

    return light;
}

Light GetPixelizeAdditionalLight(uint i, float3 positionWS)
{
#if USE_CLUSTERED_LIGHTING
    int lightIndex = i;
#else
    int lightIndex = GetPerObjectLightIndex(i);
#endif
    return GetAdditionalPixelizePerObjectLight(lightIndex, positionWS);
}

Light GetPixelizeAdditionalLight(uint i, float3 positionWS, half4 shadowMask)
{
#if USE_CLUSTERED_LIGHTING
    int lightIndex = i;
#else
    int lightIndex = GetPerObjectLightIndex(i);
#endif
    Light light = GetAdditionalPixelizePerObjectLight(lightIndex, positionWS);

#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
    half4 occlusionProbeChannels = _AdditionalLightsBuffer[lightIndex].occlusionProbeChannels;
#else
    half4 occlusionProbeChannels = _AdditionalLightsOcclusionProbes[lightIndex];
#endif
    light.shadowAttenuation = AdditionalLightShadow(lightIndex, positionWS, light.direction, shadowMask, occlusionProbeChannels);
#if defined(_LIGHT_COOKIES)
    real3 cookieColor = SampleAdditionalLightCookie(lightIndex, positionWS);
    light.color *= cookieColor;
#endif

    return light;
}


float3 DitherColor(float3 color, float levels, float threshold)
{   
    float brightness = 0;
    //brightness = (color.r + color.g + color.b) / 3;  
    //使用HSV的V来表示亮度
    float3 hsv = RgbToHsv(color);
    brightness = hsv.z;
    //brightness = max(max(color.r, color.g), color.b);
    float prev = floor(brightness * levels) / levels;
    float next = ceil(brightness * levels) / levels;
    float progress = frac(brightness * levels);
    progress = step(threshold, progress);
    float a = max(0.01, brightness);
    float b = lerp(prev, next, progress);
    //color = color / a * b;
    hsv.z = b;
    color = HsvToRgb(hsv);
    return color;
}

float3 DitherColor2(float3 color, float levels, float threshold)
{   
    float t = DitherPower;
    float brightness = (color.r + color.g + color.b) / 3;  
    brightness = pow(brightness, t);
    float prev = floor(brightness * levels) / levels;
    float next = ceil(brightness * levels) / levels;
    float progress = frac(brightness * levels);
    progress = step(threshold, progress);
    float a = max(0.01, brightness);
    float b = lerp(prev, next, progress);
    a = pow(a, 1.0 / t);
    b = pow(b, 1.0 / t);
    color = color / a * b;
    return color;
}


float3 LightingPixelLambert_LightColorDither(float3 baseColor, float3 lightColor, float3 lightDir, float3 normal, float threshold)
{
    
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    lightColor =  DitherColor(lightColor * LDotN, GetLightLevel(), threshold) * baseColor ;
    return lightColor;
}

float3 LightingPixelLambert_LDotNDither(float3 baseColor, float3 lightColor, float3 lightDir, float3 normal, float threshold)
{
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    LDotN01 *= 0.99f;
    float level = GetLightLevel();
    float current = floor(LDotN01 * level) / level;
    float next = ceil(LDotN01 * level) / level;
    float p = frac(LDotN01 * level);
    LDotN01 = lerp(current, next, step(threshold, p));
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    lightColor = lightColor * LDotN * baseColor;
    return lightColor;
}

float3 LightingPixelLambert(float3 lightColor, float3 lightDir, float3 normal)
{
    float LDotN01 = dot(lightDir, normal) * .5 + .5;
    float LDotN = lerp(_MinLDotN, 1, LDotN01);
    float3 diffuse = lightColor * LDotN;
    return diffuse;
}

float3 LightingPixelSpecular(float3 lightColor, float3 lightDir, float3 normal, float3 viewDir, float smoothness)
{
    //float3 reflectDir = reflect(-lightDir, normal);
    float3 halfDir = normalize(lightDir + viewDir);
    float HDotN = max(0, dot(halfDir, normal));
    float3 specular = lightColor * pow(HDotN, max(0.01, smoothness));
    return specular;
}


#if RECIVE_SHADOW
float3 GetMainLightColor(float3 normal, float4 shadowCoord, float threshold, float3 viewDir, float smoothness, float2 lightingIntensity)
#else
float3 GetMainLightColor(float3 normal, float threshold, float3 viewDir, float smoothness, float2 lightingIntensity)
#endif
{
    float3 finalColor = 0;
    //主光
    Light light = GetMainLight();
#if RECIVE_SHADOW
    light.shadowAttenuation = MainLightRealtimeDitherShadow(shadowCoord, threshold);
#endif
    float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
    finalColor.rgb += LightingPixelLambert(lightColor, light.direction, normal) * lightingIntensity.x;
    finalColor.rgb += LightingPixelSpecular(lightColor, light.direction, normal, viewDir, smoothness) * lightingIntensity.y;
    return finalColor;
}

float3 GetAdditionalLightColorSum(float3 normal, float3 positionWS, float threshold, float3 viewDir, float smoothness, float2 lightingIntensity)
{
    float3 finalColor = 0;
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
    #if RECIVE_SHADOW
        Light light = GetPixelizeAdditionalLight(i, positionWS, 0);
    #else
        Light light = GetPixelizeAdditionalLight(i, positionWS);
    #endif
        float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
        float3 dir = normalize(light.direction);
        finalColor.rgb += LightingPixelLambert(lightColor, dir, normal) * lightingIntensity.x;
        finalColor.rgb += LightingPixelSpecular(lightColor, dir, normal, viewDir, smoothness) * lightingIntensity.y;
    }
    return finalColor;
}



#if RECIVE_SHADOW
float4 ApplyPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS, float4 shadowCoord, float smoothness, float2 lightingIntensity, float ao)
#else
float4 ApplyPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS, float smoothness, float2 lightingIntensity, float ao)
#endif
{
    float3 finalLightColor = float3(0, 0, 0);
    float3 viewDir = normalize(GetCameraPositionWS() - positionWS);
    float threshold = DitherThreshold(pos) * _DitherWidth;
#if RECIVE_SHADOW
    finalLightColor.rgb += GetMainLightColor(normal, shadowCoord, threshold, viewDir, smoothness, lightingIntensity);
#else
    finalLightColor.rgb += GetMainLightColor(normal, threshold, viewDir, smoothness, lightingIntensity);
#endif
    finalLightColor.rgb += GetAdditionalLightColorSum(normal, positionWS, threshold, viewDir, smoothness, lightingIntensity);
    baseColor.rgb *=  DitherColor(finalLightColor + _PixelAmbientLight , GetLightLevel(), threshold) * ao;

    return baseColor;
}

float4 ApplyWaterPixelizeLighting(float4 baseColor, float3 normal, int2 pos, float3 positionWS, float4 shadowCoord)
{
    float3 finalLightColor = float3(0, 0, 0);
    float3 viewDir = normalize(GetCameraPositionWS() - positionWS);
    {
        Light light = GetMainLight();
        light.shadowAttenuation = MainLightRealtimeDitherShadow(shadowCoord, 0);
        float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
        finalLightColor.rgb += LightingPixelLambert(lightColor, light.direction, normal);
    }
    {
        int pixelLightCount = GetAdditionalLightsCount();
        for (int i = 0; i < pixelLightCount; ++i)
        {
            Light light = GetPixelizeAdditionalLight(i, positionWS, 0);
            float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
            float3 dir = normalize(light.direction);
            finalLightColor.rgb += LightingPixelLambert(lightColor, dir, normal);
        }
    }
    baseColor.rgb *= (finalLightColor.rgb + _PixelAmbientLight);
    return baseColor;
}

//---------------------PIXELIZE_BRDF----------------------------
float3 PixelizeBDRF(float3 lightDir, float3 viewDir,
float3 normal, float3 albedo,
float3 lightColor, float roughness,
float metallic, float ao, float threshold)
{
    // float3 lightDir, viewDir, normal, albedo, lightColor;
    // float roughness, metallic, ao;
    //镜面高光项计算
    float3 f0 = 0.04f;
    f0 = lerp(f0, albedo, metallic);
    float3 halfDir = normalize(lightDir + viewDir);

    float NDF = DistributionGGX(normal, halfDir, roughness);
    float G = GeometrySmith(normal, viewDir, lightDir, roughness);
    float HDotV = max(dot(halfDir, viewDir), 0);
    float3 F = FresnelSchlick(HDotV, f0);
    float3 ks = F;
    float3 kd = 1.0 - ks;
    kd *= 1.0 - metallic;
    float3 nominator = NDF * G * F;
    float NDotV = max(dot(normal, viewDir), 0);
    float NDotL = max(dot(normal, lightDir), 0);

    NDotL = lerp(_MinLDotN, 1, NDotL * .5 + .5);
    
    float denominator = 4.0 * NDotV * NDotL + 0.001;
    float3 specular = nominator / denominator;
    specular = specular * (lerp(1, 8, SpecularIntensity));
    float3 a = (kd * albedo / PI * DiffuseIntensity + specular);

    //lightColor *= NDotL;
    lightColor = lightColor * NDotL;
    lightColor = DitherColor2(lightColor, GetLightLevel(), threshold);
    

    float3 lo = a * lightColor;
    //??? 好像有负数...
    return lo;
}

#if RECIVE_SHADOW
float4 ApplyPixelizeBRDF(float4 albedo, float3 normal, float roughness, float metallic, int2 pos, float3 positionWS, float4 shadowCoord)
#else
float4 ApplyPixelizeBRDF(float4 albedo, float3 normal, float roughness, float metallic, int2 pos, float3 positionWS)
#endif
{
    float4 color = float4(0, 0, 0, albedo.a);
    float3 viewDir = normalize(GetCameraPositionWS() - positionWS);
    float threshold = DitherThreshold(pos) * _DitherWidth;

    //mainLight
    {
        Light light = GetMainLight();
    #if RECIVE_SHADOW
        light.shadowAttenuation = MainLightRealtimeDitherShadow(shadowCoord, threshold);
    #endif
        float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
        float3 lightDir = light.direction;
        color.rgb += PixelizeBDRF(lightDir, viewDir, normal, albedo, lightColor, roughness, metallic, 1, threshold);
    }

    //additionalLight
    {
        int pixelLightCount = GetAdditionalLightsCount();
        for (int i = 0; i < pixelLightCount; ++i)
        {
            Light light = GetAdditionalLight(i, positionWS);
            float3 lightColor = light.color * light.distanceAttenuation * light.shadowAttenuation;
            float3 lightDir = light.direction;
            color.rgb += PixelizeBDRF(lightDir, viewDir, normal, albedo, lightColor, roughness, metallic, 1, threshold);
        }
    }
    return color;
}


// float3 ApplyRimLight(float3 color, float threshold, float3 viewDir, float3 normal, float3 rimColor)
// {
    
// }


#endif