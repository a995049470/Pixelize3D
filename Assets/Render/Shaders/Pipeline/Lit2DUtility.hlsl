#ifndef LIT_2D_UTILITY_INCLUDE
#define LIT_2D_UTILITY_INCLUDE

Texture2D _2DLightTex;
SamplerState sampler_2DLightTex;

float3 Get2DLightColor(float2 uv)
{
    return _2DLightTex.Sample(sampler_2DLightTex, uv).xyz;
}

#endif
