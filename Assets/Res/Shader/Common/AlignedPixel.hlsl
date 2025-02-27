#ifndef ALIGNED_PIXEL_INCLUDE
#define ALIGNED_PIXEL_INCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"
#include "Assets/Res/Shader/Common/PixelizeUniform.hlsl"


int _DiscardPixel;

struct AlignedPixelOutput
{
    float4 positionCS;
    float3 positionWS;
    float4 originPosCS;
    float3 originPosWS;
};

struct ShadowAlignedPixelOutput
{
    float4 positionCS;
    float4 originPosCS;
};

//没问题的话需要挪到
float3 PositionWSAlignedPixel(float3 positionWS)
{
    float4x4 vp = _MatrixVP_AlignedPixel;
    float4x4 invVP = _MatrixInvVP_AlignedPixel;

    float4 originPosWS = float4(positionWS, 1);
    float2 pixelSizeCS = _PixelSizeCS.xy;
    float4 originPosCS = mul(vp, originPosWS);

    float2 offsetCS = fmod(originPosCS.xy / originPosCS.w + 2 + pixelSizeCS / 2, pixelSizeCS) - pixelSizeCS / 2;
    originPosCS.xy = originPosCS.xy - offsetCS * originPosCS.w;
    originPosWS = mul(invVP, originPosCS);
    return originPosWS.xyz / originPosCS.w;
}


float4 TransformWorldToShadowCoord_AlignedPixel(float3 originPosWS, float3 positionWS)
{
#ifdef _MAIN_LIGHT_SHADOWS_CASCADE
    half cascadeIndex = ComputeCascadeIndex(positionWS);
#else
    half cascadeIndex = half(0.0);
#endif

    float4x4 vp = _MainLightWorldToShadow[cascadeIndex];
    
    float4 originShadowCoord = mul(vp, float4(originPosWS, 1.0));
    float4 shadowCoord = mul(vp, float4(positionWS, 1.0));
    float2 shadowPixelSizeCS = 2 * _ShadowPixelSize * _ShadowMap_TexelSize.xy;
    float2 offsetCS = fmod(originShadowCoord.xy / originShadowCoord.w + 2, shadowPixelSizeCS);
    shadowCoord.xy = shadowCoord.xy - offsetCS * shadowCoord.w;

    return float4(shadowCoord.xyz, 0);
}

float4 TransfromWorldToShadowHClip_AlignedPixel(float3 originPosWS, float3 positionWS, float3 normalOS)
{
    float4x4 m = GetObjectToWorldMatrix();
    float4 positionCS = TransfromWorldToShadowHClip(positionWS, normalOS);
    float4 originPosCS = TransfromWorldToShadowHClip(originPosWS, normalOS);
    float4 texelSize = _ShadowMap_TexelSize;
    float2 pixelSizeCS = 2 * _ShadowPixelSize * _ShadowMap_TexelSize.xy;
    float2 offsetCS = fmod(originPosCS.xy / originPosCS.w + 2, pixelSizeCS);
    positionCS.xy -= offsetCS * positionCS.w;
    return positionCS;
}

float3 GetObjectPosition()
{
    float4x4 m = GetObjectToWorldMatrix();
    return float3(m[0][3], m[1][3], m[2][3]);
}

AlignedPixelOutput TransformWorldToHClip_AlignedPixel(float3 vertPosWS, float3 objectPosition)
{
    AlignedPixelOutput o;
    //float4x4 m = GetObjectToWorldMatrix();
    float4x4 vp = _MatrixVP_AlignedPixel;
    float4x4 invVP = _MatrixInvVP_AlignedPixel;
    
    //float4x4 mvp = mul(vp, m);

    float4 positionCS = mul(vp, float4(vertPosWS, 1));
    
    float4 originPosWS = float4(objectPosition.xyz, 1);
    float2 pixelSizeCS = _PixelSizeCS.xy;
    float4 originPosCS = mul(vp, originPosWS);
    float2 posSS = originPosCS.xy / originPosCS.w;
    float2 offsetCS = (posSS / pixelSizeCS - round(posSS / pixelSizeCS)) * pixelSizeCS;
    positionCS.xy = positionCS.xy - offsetCS * positionCS.w;
    originPosCS.xy = originPosCS.xy - offsetCS * originPosCS.w;

    float4 positionWS = mul(invVP, positionCS);
    originPosWS = mul(invVP, originPosCS);

    
    o.positionCS = positionCS;
    o.originPosCS = originPosCS;
    o.originPosWS = originPosWS.xyz / originPosWS.w;
    o.positionWS = positionWS.xyz / positionWS.w;
    return o;
}



AlignedPixelOutput TransformObjectToHClip_AlignedPixel(float3 positionOS, float3 objectPosition)
{
    
    float4x4 m = GetObjectToWorldMatrix();
    float3 positionWS = mul(m, float4(positionOS, 1)).xyz;
    return TransformWorldToHClip_AlignedPixel(positionWS, objectPosition);
}



void DiscardPixel(int2 pos, int2 originPos)
{
    if(_DiscardPixel)
    {
        pos -= originPos;
        //防止负数..
        if(any(imod(pos, _PixelSize) - _PixelSize / 2) ) discard;
    }
}

void DiscardShadowMapPixel(int2 pos, int2 originPos)
{
    if(_DiscardPixel)
    {
        pos -= originPos;
        //防止负数..
        if(any(imod(pos, _ShadowPixelSize) - _ShadowPixelSize / 2) ) discard;
    }
}


int2 CalculatePixelPos(int2 pos, int2 originPos)
{
    //防止负数
    return (pos - originPos + _PixelSize * 65536) / _PixelSize - 65536;
}

int2 CalculateShadowMapPixelPos(int2 pos, int2 originPos)
{
    return pos - originPos - imod(pos - originPos, _ShadowPixelSize) + _ShadowPixelSize / 2;
}


#endif