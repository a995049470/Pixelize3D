#ifndef FULL_SCREEN_TRIANGLE
#define FULL_SCREEN_TRIANGLE

float4 TransformTriangleVertexToHClip(float2 positionOS)
{
    return float4(positionOS, 0, 1);
}
 
//直接输出到finalBlit不需要判断UNITY_UV_STARTS_AT_TOP
float2 TransformTriangleVertexToUV_FinalBlit(float2 positionOS)
{
    float2 uv = (positionOS + 1.0) * 0.5;
    return uv;
}

float2 TransformTriangleVertexToUV(float2 positionOS)
{
    float2 uv = (positionOS + 1.0) * 0.5;
#if UNITY_UV_STARTS_AT_TOP
    uv.y = 1.0 - uv.y;
#endif
    return uv;
}


#endif