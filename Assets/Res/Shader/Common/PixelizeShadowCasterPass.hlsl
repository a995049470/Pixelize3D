#ifndef PIXELIZE_SHADOW_CASTER_PASS
#define PIXELIZE_SHADOW_CASTER_PASS

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
#pragma shader_feature_local _ALPHA_CLIP

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"

#define ALPHA_CLIP _ALPHA_CLIP

struct appdata
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
};

struct v2f
{
    float4 position : SV_POSITION;
    float4 positionCS : TEXCOORD1;
    float2 uv : TEXCOORD2;
};

CBUFFER_START(UnityPerMaterial)
float3 _RelativePosition;
float _Cutoff;
CBUFFER_END

Texture2D _MainTex;
SamplerState sampler_MainTex;


v2f vert(appdata v)
{
    v2f o = (v2f)0;
    AlignedPixelOutput res = TransformObjectToHClip_AlignedPixel(v.positionOS.xyz, _RelativePosition);
    o.positionCS = res.positionCS;
    o.position = TransfromWorldToShadowHClip(res.positionWS, v.normalOS);
    o.uv = v.texcoord;
    return o;
}


float4 frag(v2f i) : SV_TARGET
{
    float4 col = _MainTex.Sample(sampler_MainTex, i.uv);
#if ALPHA_CLIP
    clip(col.a - _Cutoff);
#endif
    return 0;
}

#endif