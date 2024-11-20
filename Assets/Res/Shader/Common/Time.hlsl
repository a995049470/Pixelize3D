#ifndef LPIPELINE_TIME_INCLUDE
#define LPIPELINE_TIME_INCLUDE

#define FPS 16.0

float GetTime(float scale, float fps = FPS)
{
    return int(_Time.y * fps) / fps * scale;
}

#endif