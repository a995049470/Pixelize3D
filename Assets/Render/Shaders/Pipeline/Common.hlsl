#ifndef LPIPELINE_COMMON_METHOD_INCLUDE
#define LPIPELINE_COMMON_METHOD_INCLUDE



float3 ScreenPosToWorldPosition(float2 uv, float depth, float4x4 invVP)
{
    float ndcz = UNITY_NEAR_CLIP_VALUE > 0 ? depth : depth * 2.0 - 1.0;
    float4 positionCS = float4(uv * 2.0 - 1.0, ndcz, 1.0);
#if UNITY_UV_STARTS_AT_TOP
	positionCS.y = -positionCS.y;
#endif
    float4 positionWS = mul(invVP, positionCS);
    return positionWS.xyz / positionWS.w;
}


float RadicalInverse_VdC(uint bits) 
{
    bits = (bits << 16u) | (bits >> 16u);
    bits = ((bits & 0x55555555u) << 1u) | ((bits & 0xAAAAAAAAu) >> 1u);
    bits = ((bits & 0x33333333u) << 2u) | ((bits & 0xCCCCCCCCu) >> 2u);
    bits = ((bits & 0x0F0F0F0Fu) << 4u) | ((bits & 0xF0F0F0F0u) >> 4u);
    bits = ((bits & 0x00FF00FFu) << 8u) | ((bits & 0xFF00FF00u) >> 8u);
    return float(bits) * 2.3283064365386963e-10; // / 0x100000000
}
// ----------------------------------------------------------------------------
float2 Hammersley(uint i, uint N)
{
    return float2(float(i)/float(N), RadicalInverse_VdC(i));
}  

// n = cross(t1 - t0, t2 - t0)
//(p0 + x * (p1 - p0) - t0) * n = 0
bool IntersectPoint_Line_Triangle(float3 linePoint[2], float3 trianglePoint[3], out float3 p)
{
    bool isIntersect = false;
    float3 n = cross(trianglePoint[1] - trianglePoint[0], trianglePoint[2] - trianglePoint[0]);
    float a = dot(linePoint[1] - linePoint[0], n);
    if(a != 0)
    {
        float t = (dot(trianglePoint[0], n) - dot(linePoint[0], n)) / a;
        if(t > 0 && t < 1)
        {                                                                                                                                 
            p = linePoint[0] + t * (linePoint[1] - linePoint[0]);
            float m[3];
            for (int i = 0; i < 3; i++) {
                m[i] = dot(cross(trianglePoint[i] - p, trianglePoint[(i + 1) % 3] - p), n);
            }
            isIntersect = m[0] > 0 && m[1] > 0 && m[2] > 0;
        }
    }
    return isIntersect;
}

#endif