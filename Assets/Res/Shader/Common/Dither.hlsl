#ifndef PIXELIZE_DITHER_INCLUDE
#define PIXELIZE_DITHER_INCLUDE

#include "Assets/Res/Shader/Common/Common.hlsl"

int _LightLevel;
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

#endif