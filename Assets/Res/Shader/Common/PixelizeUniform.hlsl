#ifndef PIXELIZE_UNIFORM_INCLUDE
#define PIXELIZE_UNIFORM_INCLUDE

float4 _CameraOffsetVS;
float4 _PixelSizeCS;
int _PixelSize;
int _ShadowPixelSize;
float4 _RenderTarget_TexelSize;

#define _ShadowMap_TexelSize float4((1.0 / 2048).xx, (2048).xx)

float4x4 _MatrixV_AlignedPixel;
float4x4 _MatrixInvV_AlignedPixel;

float4x4 _MatrixVP_AlignedPixel;
float4x4 _MatrixInvVP_AlignedPixel;

#endif