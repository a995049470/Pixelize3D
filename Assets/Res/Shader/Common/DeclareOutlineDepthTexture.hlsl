#ifndef UNITY_DECLARE_OUTLINE_DEPTH_TEXTURE_INCLUDED
#define UNITY_DECLARE_OUTLINE_DEPTH_TEXTURE_INCLUDED
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

TEXTURE2D_X_FLOAT(_OutlineDepthTexture);
SAMPLER(sampler_OutlineDepthTexture);

float SampleSceneOutlineDepth(float2 uv)
{
    return SAMPLE_TEXTURE2D_X(_OutlineDepthTexture, sampler_OutlineDepthTexture, UnityStereoTransformScreenSpaceTex(uv)).r;
}

float LoadSceneOutlineDepth(uint2 uv)
{
    return LOAD_TEXTURE2D_X(_OutlineDepthTexture, uv).r;
}
#endif
