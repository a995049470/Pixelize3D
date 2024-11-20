#ifndef PIXELIZE_DISTORTION_PASS
#define PIXELIZE_DISTORTION_PASS

TEXTURE2D(_DistortionMap);
SAMPLER(sampler_DistortionMap);

float2 GetDistortionUV(float2 uv)
{
#if _PIXELIZE_DISTORTION
    uv += SAMPLE_TEXTURE2D(_DistortionMap, sampler_DistortionMap, uv).rg;
#endif
    return uv;
}

#endif