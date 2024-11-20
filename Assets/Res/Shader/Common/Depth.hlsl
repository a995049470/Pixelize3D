#ifndef LPIPELINE_DEPTH_INCLUDE
#define LPIPELINE_DEPTH_INCLUDE

#include "Packages\com.unity.render-pipelines.universal\ShaderLibrary\DeclareDepthTexture.hlsl"

float SampleSceneEyeDepth(float2 uv)
{
    float depth = SampleSceneDepth(uv);
    float eyeDepth = unity_OrthoParams.w == 0 ? LinearEyeDepth(depth, _ZBufferParams) : LinearDepthToEyeDepth(depth);
    return eyeDepth;
}

float CalculateFragEyeDepth(float4 position)
{
    return unity_OrthoParams.w == 0 ? position.w : LinearDepthToEyeDepth(position.z);
}

#endif