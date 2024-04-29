#ifndef LPIPELINE_BRDF_PERCOMPUTE_PASS
#define LPIPELINE_BRDF_PERCOMPUTE_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
  
};

struct Varyings
{
    float4 positionCS : POSITION;
    float4 positionSS : TEXCOORD0;
    float2 uv :  TEXCOORD1;
};


Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;

    o.positionCS = TransformObjectToHClip(i.positionOS);
    o.positionSS = ComputeScreenPos(o.positionCS);
    o.uv = i.texcoord;
    return o;
}

#include "SpecularBRDFLUT.hlsl"

float2 Fragment(Varyings i) : SV_TARGET
{
    float2 uv = i.uv;
    return float2(integrateBRDF(uv.x, uv.y));
}



#endif