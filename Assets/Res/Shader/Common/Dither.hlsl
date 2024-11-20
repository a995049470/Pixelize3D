#ifndef PIXELIZE_DITHER_INCLUDE
#define PIXELIZE_DITHER_INCLUDE

#include "Assets/Res/Shader/Common/Common.hlsl"
#include "Assets\Res\Shader\Common\PixelizeUniform.hlsl"

float DitherThreshold(int2 pos)
{
    const float m[16] = 
    {
        0, 8, 2, 10,
        12, 4, 14, 6,
        3, 11, 1, 9,
        15, 7, 13, 5
    };
    //防止pos负数
    pos = imod(pos, 4);
    return m[pos.x + pos.y * 4] / 16.0f;
}

void Unity_Dither_Pixel(float In, int2 pos, float size = 0.3)
{
    float2 uv = pos * size / _PixelSize;
    float DITHER_THRESHOLDS[16] = {
        1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
        13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
        4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
        16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
    };
    uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
    float Out = In - DITHER_THRESHOLDS[index];
    clip(Out);
}


void Unity_Dither_Screen(float In, float2 ScreenPosition, float size = 0.3)
{
    float2 uv = ScreenPosition.xy * size * _ScreenParams.xy / _PixelSize;
    float DITHER_THRESHOLDS[16] = {
        1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
        13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
        4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
        16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
    };
    uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
    float Out = In - DITHER_THRESHOLDS[index];
    clip(Out);
}


#endif