//一些常用的方法
#ifndef THE_XRPG_HLSL_GENERAL_METHOD
#define THE_XRPG_HLSL_GENERAL_METHOD

#include "RandomNoise.hlsl"

float3 ScreenPosToWorldPositionWS(float2 uv, float depth)
{
    float ndcz = UNITY_NEAR_CLIP_VALUE > 0 ? depth : depth * 2.0 - 1.0;
    float4 positionCS = float4(uv * 2.0 - 1.0, ndcz, 1.0);
#if UNITY_UV_STARTS_AT_TOP
	positionCS.y = -positionCS.y;
#endif
    float4 positionWS = mul(unity_MatrixInvVP, positionCS);
    return positionWS.xyz / positionWS.w;
}

float ConvertToDepthValue(float depth)
{
    #if UNITY_REVERSED_Z
        return lerp(1 / _ZBufferParams.w / (1 + _ZBufferParams.x), 1 / _ZBufferParams.w, depth);
    #else
        return lerp(1 / _ZBufferParams.w, _ZBufferParams.y / _ZBufferParams.w, depth);
    #endif
}

float3 mod2D289(float3 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float2 mod2D289(float2 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float3 permute(float3 x)
{
    return mod2D289(((x * 34.0) + 1.0) * x);
}
// -1 ~ 1
float snoise(float2 v)
{
    const float4 C = float4(0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439);
    float2 i = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);
    float2 i1;
    i1 = (x0.x > x0.y) ? float2(1.0, 0.0): float2(0.0, 1.0);
    float4 x12 = x0.xyxy + C.xxzz;
    x12.xy -= i1;
    i = mod2D289(i);
    float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));
    float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
    m = m * m;
    m = m * m;
    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;
    m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
    float3 g;
    g.x = a0.x * x0.x + h.x * x0.y;
    g.yz = a0.yz * x12.xz + h.yz * x12.yw;
    return 130.0 * dot(m, g);
}


float3 mod3D289(float3 x)
{
    return x - floor(x / 289.0) * 289.0;
}
float4 mod3D289(float4 x)
{
    return x - floor(x / 289.0) * 289.0;
}
float4 permute(float4 x)
{
    return mod3D289((x * 34.0 + 1.0) * x);
}
float4 taylorInvSqrt(float4 r)
{
    return 1.79284291400159 - r * 0.85373472095314;
}
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



float dither(float value, float levels)
{
    const float diter[25] = 
    {
        0, 14, 22, 5, 8, 
        18, 9, 1, 19, 13, 
        6, 24, 16, 7, 23,
        21, 2, 12, 20, 3,
        10, 15, 4, 11, 17
    };
    
    float v = value * levels;
    float last = floor(v);
    float next = ceil(v);
    float p = frac(v);
    int i = p * 25;
    float d = diter[i] / 24;
    float res = (last * d + next * (1 - d)) / levels;
    return res;
}

float voronoi(float3 value)
{
    float3 baseCell = floor(value);
    
    //first pass to find the closest cell
    float minDistToCell = 10;
    float3 toClosestCell;
    float3 closestCell;

    for (int x1 = -1; x1 <= 1; x1++)
    {
        for (int y1 = -1; y1 <= 1; y1++)
        {
            for (int z1 = -1; z1 <= 1; z1++)
            {
                float3 cell = baseCell + float3(x1, y1, z1);
                float3 cellPosition = cell + rand3dTo3d(cell);
                float3 toCell = cellPosition - value;
                float distToCell = 10;
                if (toCell.y > 0)
                {
                    distToCell = length(toCell);
                }
              
                minDistToCell = min(distToCell, minDistToCell);
            }
        }
    }
    return minDistToCell;
}


inline float noise_randomValue(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}
inline float noise_interpolate(float a, float b, float t)
{
    return(1.0 - t) * a + (t * b);
}
inline float valueNoise(float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);
    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = noise_randomValue(c0);
    float r1 = noise_randomValue(c1);
    float r2 = noise_randomValue(c2);
    float r3 = noise_randomValue(c3);
    float bottomOfGrid = noise_interpolate(r0, r1, f.x);
    float topOfGrid = noise_interpolate(r2, r3, f.x);
    float t = noise_interpolate(bottomOfGrid, topOfGrid, f.y);
    return t;
}

float SimpleNoise(float2 UV)
{
    float t = 0.0;
    float freq = pow(2.0, float(0));
    float amp = pow(0.5, float(3 - 0));
    t += valueNoise(UV / freq) * amp;
    freq = pow(2.0, float(1));
    amp = pow(0.5, float(3 - 1));
    t += valueNoise(UV / freq) * amp;
    freq = pow(2.0, float(2));
    amp = pow(0.5, float(3 - 2));
    t += valueNoise(UV / freq) * amp;
    return t;
}

float3 random3D (float3 x) {
    return frac(sin(x) * 323434.34344);
}

float2 random2D (float2 x) {
    return frac(sin(x) * 323434.34344);
}
float random1D (float x) {
    return frac(sin(x) * 323434.34344);
}

float yFromT(float t, float E, float F, float G, float H)
{
    float y = E * (t * t * t) + F * (t * t) + G * t + H;
    return y;
}

float xFromT(float t, float A, float B, float C, float D)
{
    float x = A * (t * t * t) + B * (t * t) + C * t + D;
    return x;
}

// Helper functions:
float slopeFromT(float t, float A, float B, float C)
{
    float dtdx = 1.0 / (3.0 * A * t * t + 2.0 * B * t + C);
    return dtdx;
}

#define nRefinementIterations 3

float cubicBezier(float x, float a, float b, float c, float d)
{

    float y0a = 0.00; // initial y
    float x0a = 0.00; // initial x
    float y1a = b;    // 1st influence y
    float x1a = a;    // 1st influence x
    float y2a = d;    // 2nd influence y
    float x2a = c;    // 2nd influence x
    float y3a = 1.00; // final y
    float x3a = 1.00; // final x

    float A = x3a - 3 * x2a + 3 * x1a - x0a;
    float B = 3 * x2a - 6 * x1a + 3 * x0a;
    float C = 3 * x1a - 3 * x0a;
    float D = x0a;

    float E = y3a - 3 * y2a + 3 * y1a - y0a;
    float F = 3 * y2a - 6 * y1a + 3 * y0a;
    float G = 3 * y1a - 3 * y0a;
    float H = y0a;

    // Solve for t given x (using Newton-Raphelson), then solve for y given t.
    // Assume for the first guess that t = x.
    float currentt = x;
    for (int i = 0; i < nRefinementIterations; i++)
    {
        float currentx = xFromT(currentt, A, B, C, D);
        float currentslope = slopeFromT(currentt, A, B, C);
        currentt -= (currentx - x) * (currentslope);
        currentt = clamp(currentt, 0, 1);
    }

    float y = yFromT(currentt, E, F, G, H);
    return y;
}

#define FBM_LOOP_COUNT 3
float fbm(float3 uv)
{
    float noise = 0;
    float sumWeight = 0;
    float weight = 1;
    float a = 1;
    float t = 2.7;
    for (int i = 0; i < FBM_LOOP_COUNT; i++) {
        noise += snoise(uv) * weight;
        sumWeight += weight;
        weight /= t;
        uv *= t;
    }
    noise /= sumWeight;

    // float noise = snoise(uv) + snoise(2 * uv) * 0.5 + snoise(4 * uv) * 0.25 + snoise(8 * uv) * 0.125;
    // noise /= 1.875;
    return noise;
}


#endif