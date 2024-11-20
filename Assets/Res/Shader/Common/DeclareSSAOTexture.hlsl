#ifndef DECLARE_SSAO_TEXTURE_INCLUDE
#define DECLARE_SSAO_TEXTURE_INCLUDE

// TEXTURE2D_X(_ScreenSpaceOcclusionTexture);
// SAMPLER(sampler_ScreenSpaceOcclusionTexture);
int _VaildSSAOTexture;

float SampleSSAO(float2 uv)
{
    return SAMPLE_TEXTURE2D_X(_ScreenSpaceOcclusionTexture, sampler_ScreenSpaceOcclusionTexture, UnityStereoTransformScreenSpaceTex(uv)).r;
}

float3 ApplySSAO(float3 color, float2 uv)
{
    if(_VaildSSAOTexture)
    {
        float ao = SampleSSAO(uv);
        color = lerp(0, color, ao);
    }
    return color;
}

#endif