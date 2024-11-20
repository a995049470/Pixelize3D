#ifndef LPIPELINE_WIND_INCLUDE
#define LPIPELINE_WIND_INCLUDE

#include "Assets/Res/Shader/Common/Common.hlsl"

float3 _WindDir;
float _BaseWindStrength;
float _NoiseWindStrength;
float _WindNoiseTile;
float _WindSpeed;
float _WindFPS;

float GetWindStrength(float3 originPos)
{
    float2 uv = originPos.xz * _WindNoiseTile;
    float time = _Time.y;
    float fps = max(_WindFPS, 0.1);
    time = int(_Time.y * fps) / fps;
    uv += _WindSpeed * time * normalize(_WindDir.xz);
    float strength = _BaseWindStrength + snoise(float3(uv, 0)) * _NoiseWindStrength;
    return strength;
}

float3 VertOffsetByWind(float3 positionWS, float groundY, float height, float bend, float3 originPos)
{
    float3 windDir = normalize(_WindDir);
    float windStrength = GetWindStrength(originPos);
    float curve = pow((positionWS.y - groundY) / height, bend);
    positionWS += windDir * windStrength * curve;
    return positionWS;
}

#endif