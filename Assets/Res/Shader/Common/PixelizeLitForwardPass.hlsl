#ifndef PIXELIZE_LIT_FORWARD_PASS
#define PIXELIZE_LIT_FORWARD_PASS

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma shader_feature_local _RECIVE_SHADOW
#pragma shader_feature_local _ALPHA_CLIP

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/PixelizeLighting.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"

#define RECIVE_SHADOW _RECIVE_SHADOW
#define ALPHA_CLIP _ALPHA_CLIP

struct appdata
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float3 normalOS : NORMAL;
    float4 color : COLOR0;
};

struct v2f
{
    float4 position     : SV_POSITION;
    float2 uv           : TEXCOORD0;
    float3 normalWS     : NORMAL;
    float4 color        : COLOR;
    int2 originPos      : TEXCOORD1;
#if RECIVE_SHADOW 
    float4 shadowCoord  : TEXCOORD2;
#endif
    float3 positionWS   : TEXCOORD3;

};

CBUFFER_START(UnityPerMaterial)
float4 _Color;
float4 _OutlineColor;
float3 _RelativePosition;
float _Cutoff;
CBUFFER_END

Texture2D _MainTex;
SamplerState sampler_MainTex;


v2f vert(appdata v)
{
    v2f o = (v2f)0;
    o.uv = v.uv;
    AlignedPixelOutput result = TransformObjectToHClip_AlignedPixel(v.positionOS.xyz, _RelativePosition);
    o.position = result.positionCS;
    float4 originPosCS = result.originPosCS;
    float2 originPosSS = (originPosCS.xy / originPosCS.w) * .5 + .5;
#if UNITY_UV_STARTS_AT_TOP
    originPosSS.y = 1.0 - originPosSS.y;
#endif
#if RECIVE_SHADOW
    o.shadowCoord = TransformWorldToShadowCoord(result.positionWS);
#endif
    //cao! 裁剪空间坐标转世界空间坐标需要注意y的朝向
    o.originPos = int2(originPosSS * _RenderTarget_TexelSize.zw);
    
    o.normalWS = TransformObjectToWorldNormal(v.normalOS.xyz);
    o.color = v.color;
    o.positionWS = result.positionWS;
    return o;
}


float4 frag(v2f i) : SV_Target
{
    float4 col = _MainTex.Sample(sampler_MainTex, i.uv) * i.color;
    col *= _Color;
    DiscardPixel(int2(i.position.xy), 0);
#if RECIVE_SHADOW
    float4 shadowCoord = i.shadowCoord; 
    col = ApplyPixelizeLighting(col, i.normalWS, CalculatePixelPos(i.position.xy, i.originPos), i.positionWS,  shadowCoord);
#else
    col = ApplyPixelizeLighting(col, i.normalWS, CalculatePixelPos(i.position.xy, i.originPos), i.positionWS);
#endif
#if ALPHA_CLIP
    clip(col.a - _Cutoff);
#endif
    return col;
}



#endif