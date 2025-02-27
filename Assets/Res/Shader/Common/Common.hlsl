#ifndef COMMON_HLSL_INCLUDE
#define COMMON_HLSL_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//TODO:考虑对负数取模的情况?
//TODO:希望余数和除数正负性相同?
int imod(int x, int a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}

int2 imod(int2 x, int2 a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}

int3 imod(int3 x, int3 a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}

int4 imod(int4 x, int4 a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}



inline float OrthoLinearEyeDepth(float depth)
{
    #if UNITY_REVERSED_Z
        depth = 1 - depth;
    #endif
    return lerp(_ProjectionParams.y, _ProjectionParams.z, depth);
}

float LinearStep(float a, float b, float x)
{
    return saturate((x - a) / (b - a));
}

float2 LinearStep(float2 a, float2 b, float2 x)
{
    return saturate((x - a) / (b - a));
}

float3 LinearStep(float3 a, float3 b, float3 x)
{
    return saturate((x - a) / (b - a));
}

float4 LinearStep(float4 a, float4 b, float4 x)
{
    return saturate((x - a) / (b - a));
}

float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

//-1 ~ 1
float snoise(float3 v)
{
    const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);
    float3 i = floor(v + dot(v, C.yyy));
    float3 x0 = v - i + dot(i, C.xxx);
    float3 g = step(x0.yzx, x0.xyz);
    float3 l = 1.0 - g;
    float3 i1 = min(g.xyz, l.zxy);
    float3 i2 = max(g.xyz, l.zxy);
    float3 x1 = x0 - i1 + C.xxx;
    float3 x2 = x0 - i2 + C.yyy;
    float3 x3 = x0 - 0.5;
    i = mod3D289(i);
    float4 p = permute(permute(permute(i.z + float4(0.0, i1.z, i2.z, 1.0)) + i.y + float4(0.0, i1.y, i2.y, 1.0)) + i.x + float4(0.0, i1.x, i2.x, 1.0));
    float4 j = p - 49.0 * floor(p / 49.0);  // mod(p,7*7)
    float4 x_ = floor(j / 7.0);
    float4 y_ = floor(j - 7.0 * x_);  // mod(j,N)
    float4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
    float4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;
    float4 h = 1.0 - abs(x) - abs(y);
    float4 b0 = float4(x.xy, y.xy);
    float4 b1 = float4(x.zw, y.zw);
    float4 s0 = floor(b0) * 2.0 + 1.0;
    float4 s1 = floor(b1) * 2.0 + 1.0;
    float4 sh = -step(h, 0.0);
    float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
    float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
    float3 g0 = float3(a0.xy, h.x);
    float3 g1 = float3(a0.zw, h.y);
    float3 g2 = float3(a1.xy, h.z);
    float3 g3 = float3(a1.zw, h.w);
    float4 norm = taylorInvSqrt(float4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3)));
    g0 *= norm.x;
    g1 *= norm.y;
    g2 *= norm.z;
    g3 *= norm.w;
    float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
    m = m * m;
    m = m * m;
    float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
    return 42.0 * dot(m, px);
}

//angle为角度制
float2 Rotate(float2 uv, float2 center, float angle, float2 flip)
{
    float rad = angle / 180 * PI;
    float s = sin(rad);
    float c = cos(rad);
    float2x2 m = float2x2(
        c, -s,
        s, c
    );
    uv = mul(m, uv - center) + center;
    uv = (1 - 2 * flip) * uv + flip;
    return uv;
}

#endif