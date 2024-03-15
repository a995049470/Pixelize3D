#ifndef PIXELIZE_OUTLINE_PASS
#define PIXELIZE_OUTLINE_PASS


#pragma vertex vert
#pragma fragment frag
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/PixelizeLighting.hlsl"

struct appdata
{
    float4 positionOS           : POSITION;
    float3 normalOS             : NORMAL;
};

struct v2f
{
    float4 position             : SV_POSITION;
    float originPos             : TEXCOORD1;
    float3 positionWS           : TEXCOORD2;
    float3 normalWS             : TEXCOORD3;
};

CBUFFER_START(UnityPerMaterial)
float4 _Color;
float4 _OutlineColor;
float3 _RelativePosition;
float _Outline;
CBUFFER_END

Texture2D _MainTex;
SamplerState sampler_MainTex;


v2f vert(appdata v)
{
    v2f o = (v2f)0;
    AlignedPixelOutput result = TransformObjectToHClip_AlignedPixel(v.positionOS.xyz, _RelativePosition);
    o.position = result.positionCS;
    float3 originPosWS = result.originPosWS;
    {
        o.originPos = originPosWS.x * 17.17 + originPosWS.y * 123.131 - originPosWS.z * 319.7931;
    }
    o.normalWS = TransformObjectToWorldNormal(v.normalOS);
    o.positionWS = result.positionWS;

    return o;
}

int RGBToInt(float4 color)
{
    int r = uint(color.r * 255);
    int g = uint(color.g * 255);
    int b = uint(color.b * 255);
    return r | (g << 8) | (b << 16);
}

float4 frag(v2f i) : SV_TARGET
{
    DiscardPixel(int2(i.position.xy), 0);
    float3 outlineColor = ApplyPixelizeLighting(_OutlineColor, i.normalWS, i.position.xy, i.positionWS);
    float4 color = float4(outlineColor, i.originPos) * _Outline;
    return color;
}

#endif