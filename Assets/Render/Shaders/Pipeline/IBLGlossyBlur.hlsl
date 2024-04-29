#ifndef LPIPELINE_IBL_ROUGHNESS_BLUR_PASS
#define LPIPELINE_IBL_ROUGHNESS_BLUR_PASS

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
    float3 dir : TEXCOORD1;
};

TextureCube _MainTex; SamplerState sampler_MainTex;
float _Roughness;

Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    float2 texcoord = i.texcoord;
#if UNITY_UV_STARTS_AT_TOP
	texcoord.y = 1.0 - texcoord.y;
#endif
    o.positionCS = float4(texcoord * 2.0 - 1.0, 0, 1);
    o.uv = i.texcoord;
    o.dir = normalize(i.positionOS.xyz);
    return o;
}

#include "BRDF.hlsl"

float4 Fragment(Varyings i) : SV_TARGET
{
    //开始进行半球面上的采样
    float roughness = _Roughness;
    //return roughness;
    float3 N = i.dir; 
    float3 R = N;
    float3 V = R;
    const uint SAMPLE_COUNT = 1024u;
    float totalWeight = 0.0;   
    float3 prefilteredColor = 0.0;   
    for(uint i = 0u; i < SAMPLE_COUNT; ++i)
    {
        float2 Xi = Hammersley(i, SAMPLE_COUNT);
        float3 H  = ImportanceSampleGGX(Xi, N, roughness);
        float3 L  = normalize(2.0 * dot(V, H) * H - V);

        float NdotL = max(dot(N, L), 0.0);
        if(NdotL > 0.0)
        {
            prefilteredColor += _MainTex.Sample(sampler_MainTex, L).rgb * NdotL;
            totalWeight      += NdotL;
        }
    }
    prefilteredColor = prefilteredColor / totalWeight;
    return float4(prefilteredColor, 1);
}



#endif