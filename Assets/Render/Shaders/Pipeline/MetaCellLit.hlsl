#ifndef LPIPELINE_META_CELL_LIT_PASS
#define LPIPELINE_META_CELL_LIT_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
    float4 tangentOS : TANGENT;
    float3 normalOS : NORMAL;
};

struct Varyings
{
    float4 positionCS : POSITION;
    float3 cellLightUV : TEXCOORD0;
    float3 positionWS : TEXCOORD1;
    float2 uv : TEXCOORD2;
    float3 tangentWS : TEXCOORD3;
    float3 bitangentWS : TEXCOORD4;
    float3 positionWS : TEXCOORD5;
};

float3 _Origin;
float3 _BlockNum;
float3 _BlockSize;
Texture3D _GlobalLightColorTexture; SamplerState sampler_GlobalLightColorTexture;
Texture2D _MainTex; SamplerState sampler_MainTex;
Texture2D _AlbedoTex; SamplerState sampler_AlbedoTex;
Texture2D _NormalTex; SamplerState sampler_NormalTex;
Texture2D _MetallicTex; SamplerState sampler_MetallicTex;

Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    float3 positionWS = TransformObjectToWorld(i.positionOS);
    float3 cellLightUV = (positionWS - _Origin) / (_BlockNum * _BlockSize);
    o.positionWS = positionWS;
    o.cellLightUV = cellLightUV;
    o.positionCS = TransformObjectToHClip(i.positionOS);
    o.uv = i.texcoord;
    return o;
}

float4 Fragment(Varyings i) : SV_TARGET
{
    float3 albedo = _AlbedoTex.Sample(sampler_AlbedoTex, i.uv).rgb * Albedo;
    float3 normalTS = _NormalTex.Sample(sampler_NormalTex, i.uv).rgb;
    normalTS = normalTS * 2.0 - 1.0;
    float3x3 t2w = float3x3(i.tangentWS, i.bitangentWS, i.normalWS);
    float3 normalWS = mul(normalTS, t2w);
    float3 lightColor = _GlobalLightColorTexture.Sample(sampler_GlobalLightColorTexture, i.cellLightUV).xyz;
    //lightColor = step(0, i.cellLightUV - .5);
    float3 color = _MainTex.Sample(sampler_MainTex, i.uv);
    lightColor /= (1 + lightColor);
    color *= lightColor;
    return float4(color, 1);
}

#endif