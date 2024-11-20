#ifndef LPIPELINE_UNLIT_PASS
#define LPIPELINE_UNLIT_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
  
};

struct Varyings
{
    float4 positionCS : POSITION;
    float2 uv : TEXCOORD0;
};

Texture2D _AlbedoTex; SamplerState sampler_AlbedoTex;


Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
    o.uv = i.texcoord;
    
    return o;
}

float4 Fragment(Varyings i) : SV_TARGET
{
    float4 albedo = _AlbedoTex.Sample(sampler_AlbedoTex, i.uv);
    return albedo;
}


#endif