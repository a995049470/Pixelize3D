#ifndef LPIPELINE_TONE_MAPPING_PASS
#define LPIPELINE_TONE_MAPPING_PASS

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

Texture2D _MainTex; SamplerState sampler_MainTex;
float _AdaptedLum;

Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
    o.uv = i.texcoord;
    
    return o;
}
float3 ACESToneMapping(float3 color, float adapted_lum)
{
	const float A = 2.51f;
	const float B = 0.03f;
	const float C = 2.43f;
	const float D = 0.59f;
	const float E = 0.14f;

	color *= adapted_lum;
	return (color * (A * color + B)) / (color * (C * color + D) + E);
}

float4 Fragment(Varyings i) : SV_TARGET
{
    float4 color = _MainTex.Sample(sampler_MainTex, i.uv);
    
    color.rgb = ACESToneMapping(color.rgb, _AdaptedLum);
    
    return color;
}



#endif