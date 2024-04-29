#ifndef LPIPELINE_META_CELL_GI_METHOD
#define LPIPELINE_META_CELL_GI_METHOD

float3 _Origin;
float3 _BlockNum;
float3 _BlockSize;
Texture3D _GlobalLightColorTexture;
SamplerState sampler_GlobalLightColorTexture;
float4 _GlobalLightColorTexture_TexelSize;

float3 GetMetaCellGILightColor(float3 positionWS, float3 normal)
{
    float3 cellLightUV = (positionWS - _Origin) / (_BlockNum * _BlockSize);
    //cellLightUV += normal * 0 * (1. / 64.);
    cellLightUV = saturate(cellLightUV);
    float3 lightColor = _GlobalLightColorTexture.Sample(sampler_GlobalLightColorTexture, cellLightUV).xyz;
    lightColor /= (1 + lightColor);
    return lightColor;
}

float GetMetaCellGIVisiable(float3 positionWS, float3 normal)
{
    float3 cellLightUV = (positionWS - _Origin) / (_BlockNum * _BlockSize);
    cellLightUV = saturate(cellLightUV);
    float3 lightColor = _GlobalLightColorTexture.Sample(sampler_GlobalLightColorTexture, cellLightUV).xyz;
    //lightColor /= (1 + lightColor);
    float visiable = max(max(lightColor.r, lightColor.g), lightColor.b);
    visiable = saturate(visiable * 2);
    
    return visiable;
}

#endif