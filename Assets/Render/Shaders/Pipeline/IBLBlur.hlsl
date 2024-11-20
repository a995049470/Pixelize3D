#ifndef LPIPELINE_IBL_BLUR_PASS
#define LPIPELINE_IBL_BLUR_PASS

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


float4 Fragment(Varyings i) : SV_TARGET
{
    //开始进行半球面上的采样
    float3 normal = i.dir;
    float3 up = float3(0.0, 1.0, 0.0);
    float3 right = cross(up, normal);
    up = cross(normal, right);
    
    float3 irradiance = 0;
    float sampleDelta = 0.025;
    float weightSum = 0;
    //半球面采样
    for(float phi = 0.0; phi < 2.0 * PI; phi += sampleDelta)
    {
        for(float theta = 0.0; theta < 0.5 * PI; theta += sampleDelta)
        {
            // spherical to cartesian (in tangent space)
            float3 tangentSample = float3(sin(theta) * cos(phi),  sin(theta) * sin(phi), cos(theta));
            // tangent space to world
            float3 sampleVec = tangentSample.x * right + tangentSample.y * up + tangentSample.z * normal; 

            //我们将采样的颜色值乘以系数 cos(θ) ，因为较大角度的光较弱，
            //而系数 sin(θ) 则用于权衡较高半球区域的较小采样区域的贡献度。
            float weight = cos(theta) * sin(theta);
            irradiance += _MainTex.Sample(sampler_MainTex, sampleVec).rgb * weight;
            weightSum += weight;
        }
    }
    irradiance = irradiance * (1.0 / weightSum);
    return float4(irradiance, 1);
}



#endif