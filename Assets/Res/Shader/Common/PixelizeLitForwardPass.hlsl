#ifndef PIXELIZE_LIT_FORWARD_PASS
#define PIXELIZE_LIT_FORWARD_PASS

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS 
#pragma multi_compile _ _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma multi_compile_instancing
//#pragma multi_compile_fragment _ _PIXELIZE_BRDF
#pragma shader_feature_local _RECIVE_SHADOW
#pragma shader_feature_local _ALPHA_CLIP
#pragma shader_feature_local _NO_BRDF
#pragma shader_feature_local _VIRTUAL_MORMAL
#pragma shader_feature_local _REJECT_LIGHT_LEVEL
#pragma shader_feature_local _HSV_OFFSET
#pragma shader_feature_local _WIND
#pragma shader_feature_local _LIGHTING 
//#pragma shader_feature_local _CUSTOM_OBJECT_POSITION

#define RECIVE_SHADOW _RECIVE_SHADOW
#define ALPHA_CLIP _ALPHA_CLIP
#define BRDF _PIXELIZE_BRDF
#define VIRTUAL_MORMAL _VIRTUAL_MORMAL
#define USE_NORMAL_MAP _VIRTUAL_MORMAL
#define REJECT_LIGHT_LEVEL _REJECT_LIGHT_LEVEL
#define HSV_OFFSET _HSV_OFFSET
#define WIND _WIND
#define LIGHTING _LIGHTING
//#define USE_CUSTOM_OBJECT_POSITION _CUSTOM_OBJECT_POSITION

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/PixelizeLighting.hlsl"
#include "Assets/Res/Shader/Common/PixelizeShadow.hlsl"
#include "Assets/Res/Shader/Common/Wind.hlsl"


struct appdata
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float3 normalOS : NORMAL;
    float4 color : COLOR0;
#if USE_NORMAL_MAP
    float4 tangentOS : TANGENT;
#endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 position     : SV_POSITION;
    float2 uv           : TEXCOORD0;
    float3 normalWS     : NORMAL;
#if USE_NORMAL_MAP
    float4 tangentWS    : TANGENT;
#endif
    float4 color        : COLOR;
    int2 originPos      : TEXCOORD1;
#if RECIVE_SHADOW 
    float4 shadowCoord  : TEXCOORD2;
#endif
    float3 positionWS   : TEXCOORD3;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};


UNITY_INSTANCING_BUFFER_START(UnityInstancePerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
    UNITY_DEFINE_INSTANCED_PROP(float4, _OutlineColor)
    UNITY_DEFINE_INSTANCED_PROP(float3, _RelativePosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
    UNITY_DEFINE_INSTANCED_PROP(float4, _HitColor)
    UNITY_DEFINE_INSTANCED_PROP(float4, _EmissionColor)
    UNITY_DEFINE_INSTANCED_PROP(float, _Roughness)
    UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
    UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
    UNITY_DEFINE_INSTANCED_PROP(float, _SpecularIntensity)
    UNITY_DEFINE_INSTANCED_PROP(float, _DiffuseIntensity)
    UNITY_DEFINE_INSTANCED_PROP(float, _NormalScale)
    UNITY_DEFINE_INSTANCED_PROP(float3, _VirtualNormaLightDir)
    UNITY_DEFINE_INSTANCED_PROP(float3, _ObjectPosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _UseCustomObjectPosition)
    UNITY_DEFINE_INSTANCED_PROP(float, _H)
    UNITY_DEFINE_INSTANCED_PROP(float, _S)
    UNITY_DEFINE_INSTANCED_PROP(float, _V)
    UNITY_DEFINE_INSTANCED_PROP(float4, _MainTex_ST)
    UNITY_DEFINE_INSTANCED_PROP(float4, _NormalMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float4, _AOMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float, _PlantHeight)
    UNITY_DEFINE_INSTANCED_PROP(float, _Bend)
    UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexRotate)
UNITY_INSTANCING_BUFFER_END(UnityInstancePerMaterial)

#define _Color UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Color)
#define _OutlineColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _OutlineColor)
#define _RelativePosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _RelativePosition)
#define _Cutoff UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Cutoff)
#define _HitColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _HitColor)
#define _EmissionColor UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _EmissionColor)
#define _Roughness UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Roughness)
#define _Metallic UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Metallic)
#define _Smoothness UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Smoothness)
#define _SpecularIntensity UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _SpecularIntensity)
#define _DiffuseIntensity UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _DiffuseIntensity)
#define _NormalScale UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _NormalScale)
#define _VirtualNormaLightDir UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _VirtualNormaLightDir)
#define _ObjectPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _ObjectPosition)
#define _UseCustomObjectPosition UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _UseCustomObjectPosition)
#define _H UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _H)
#define _S UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _S)
#define _V UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _V)
#define _MainTex_ST UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _MainTex_ST)
#define _NormalMap_ST UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _NormalMap_ST)
#define _AOMap_ST UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _AOMap_ST)
#define _PlantHeight UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _PlantHeight)
#define _Bend UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _Bend)
#define _MainTexRotate UNITY_ACCESS_INSTANCED_PROP(UnityInstancePerMaterial, _MainTexRotate)

Texture2D _MainTex;
SamplerState sampler_MainTex;
Texture2D _AOMap;
SamplerState sampler_AOMap;
Texture2D _NormalMap;
SamplerState sampler_NormalMap;
Texture2D _EmissionTex;
SamplerState sampler_EmissionTex;

v2f vert(appdata v)
{
    v2f o = (v2f)0;
    UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
    o.uv = v.uv;
    float3 objectPositon = lerp(GetObjectPosition(), _ObjectPosition, _UseCustomObjectPosition);
    float3 positionWS = TransformObjectToWorld(v.positionOS.xyz);
#if WIND
    float height = _PlantHeight;
    float bend = _Bend;
    positionWS = VertOffsetByWind(positionWS, 0, height, bend, GetObjectPosition());
#endif
    AlignedPixelOutput result = TransformWorldToHClip_AlignedPixel(positionWS, objectPositon);
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
    o.originPos = int2(originPosSS * _RenderTarget_TexelSize.zw + 65535) - 65535;
    
    o.normalWS = TransformObjectToWorldNormal(v.normalOS.xyz);
#if USE_NORMAL_MAP
    float sign = v.tangentOS.w * GetOddNegativeScale();
    float4 tangentWS = float4(TransformObjectToWorldDir(v.tangentOS.xyz), sign);
    o.tangentWS = tangentWS;
#endif
    o.color = v.color;
    o.positionWS = result.positionWS;
    return o;
}


float4 frag(v2f i) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(i);
    float2 uv_main = i.uv;
    uv_main = uv_main * _MainTex_ST.xy + _MainTex_ST.zw;
    uv_main = Rotate(uv_main, float2(0.5, 0.5), _MainTexRotate.z, _MainTexRotate.xy);
    float4 col = _MainTex.Sample(sampler_MainTex, uv_main);
    col.rgb *= i.color.rgb * _Color.rgb;
#if HSV_OFFSET
    {
        float3 hsv = RgbToHsv(col.rgb);
        hsv += float3(_H, _S, _V);
        col.rgb = HsvToRgb(hsv);
    }
#endif

    // float3 hsv = RgbToHsv(col.rgb);
    // float3 level = float3(4, 4, 4);
    // hsv = round(hsv * level) / level;
    // col.rgb = HsvToRgb(hsv);
    
    DiscardPixel(int2(i.position.xy), 0);
    int2 pxielPos =  CalculatePixelPos(i.position.xy, i.originPos);
    //float smoothness = pow(2, _Smoothness) - 1;
    float3 normalWS = i.normalWS;
    float3 virtualNormal = 0;
#if LIGHTING 
    float smoothness = 1.0 / max(0.0001, _Roughness * _Roughness * _Roughness);
    float2 lightingIntensity = float2(
        _DiffuseIntensity,
        _SpecularIntensity
    );
    float ao = _AOMap.Sample(sampler_AOMap, i.uv * _AOMap_ST.xy + _AOMap_ST.zw).r;
#else
    float smoothness = 100000;
    float2 lightingIntensity = float2(1, 0);
    float ao = 1;
#endif
#if USE_NORMAL_MAP
    float3 normalTS = _NormalMap.Sample(sampler_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw).xyz;
    normalTS = normalTS * 2.0 - 1.0;
    normalTS.z = max(normalTS.z, 0.001);
    normalTS.xy *= (pow(2, _NormalScale) - 1);
    // normalTS.z = max(normalTS.z, 0.001);
    normalTS = normalize(normalTS);
    //normalTS = lerp(float3(0, 0, 1), normalize(normalTS), _NormalScale);
    float sgn = i.tangentWS.w;   // should be either +1 or -1
    float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz);
    float3x3 t2w = float3x3(i.tangentWS.xyz, bitangent.xyz, i.normalWS.xyz);
    virtualNormal = TransformTangentToWorld(normalTS, t2w);
#else
    virtualNormal = i.normalWS;
#endif

#if VIRTUAL_MORMAL
    float3 vdir = normalize(_VirtualNormaLightDir);
    ao *= LightingPixelLambert(1, vdir, virtualNormal).r / LightingPixelLambert(1, vdir, normalWS);
#endif

#if RECIVE_SHADOW
    float4 shadowCoord = i.shadowCoord;
    #if BRDF
    col = ApplyPixelizeBRDF(
        col, 
        normalWS, 
        _Roughness, 
        _Metallic, 
        pxielPos,
        i.positionWS,
        shadowCoord);
    #else
    col = ApplyPixelizeLighting(col, normalWS, pxielPos, i.positionWS,  shadowCoord, smoothness, lightingIntensity, ao);
    #endif
#else
    #if BRDF
    col = ApplyPixelizeBRDF(
        col, 
        normalWS, 
        _Roughness, 
        _Metallic, 
        pxielPos,
        i.positionWS
        );
    #else
    col = ApplyPixelizeLighting(col, normalWS, pxielPos, i.positionWS, smoothness, lightingIntensity, ao);
    #endif
#endif

    //col *= _Color * i.color;
    col.rgb += _HitColor.rgb;

#if ALPHA_CLIP
    clip(col.a - _Cutoff);
#endif
    col.rgb += _EmissionTex.Sample(sampler_EmissionTex, i.uv).rgb * _EmissionColor.rgb;
    
    // float4x4 iv = UNITY_MATRIX_I_V;
    // float3 forward = float3(iv[0][2], iv[1][2], iv[2][2]);
    // float FDotN = dot(forward, normalWS);
    // col.rgb = 1.0 - step(0.95, FDotN);

    return col;
}



#endif