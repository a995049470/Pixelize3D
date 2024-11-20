#ifndef PIXELIZE_WATER_PASS
#define PIXELIZE_WATER_PASS

#pragma vertex vert
#pragma fragment frag

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS 
#pragma multi_compile _ _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT


//#define REJECT_LIGHT_LEVEL 1
#define DEPTH_ONLY _DEPTH_ONLY
#define RECIVE_SHADOW 1

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Assets/Res/Shader/Common/Depth.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
#include "Assets/Res/Shader/Common/Common.hlsl"
#include "Assets/Res/Shader/Common/Time.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Assets/Res/Shader/Common/PixelizeLighting.hlsl"
#include "Assets/Res/Shader/Common/DeclareSSAOTexture.hlsl"


struct appdata
{
    float4 positionOS : POSITION;
};

struct v2f
{
    float4 position     : SV_POSITION;
    float2 uv           : TEXCOORD0;
    float4 positionSS   : TEXCOORD1;
    float3 positionWS   : TEXCOORD2;
    float4 shadowCoord  : TEXCOORD3;
};

CBUFFER_START(UnityPerMaterial)
float4 _ShallowWaterColor;
float4 _DeepWaterColor;
float _ShallowWaterDepth;
float _DeepWaterDepth;
float _WaterVisibility;

float4 _WaveColor;
float _WaveNoiseUVScaleX;
float _WaveNoiseUVScaleY;
float _WaveNoiseUVSpeedZ;
float _WaveNoiseCutoff;

CBUFFER_END


Texture2D _WaveNoiseTex;
SamplerState sampler_WaveNoiseTex;

v2f vert(appdata v)
{
    v2f o = (v2f)0;
    float3 objectPositon = float3(1, 2, 1);
    float3 positionOS = v.positionOS.xyz;
    positionOS.y += 0.0001f;
    AlignedPixelOutput result = TransformObjectToHClip_AlignedPixel(positionOS, objectPositon);
    float3 positionWS = result.positionWS;
    float4 positionCS = result.positionCS;
    o.uv = positionWS.xz;
    o.positionWS = positionWS;
    o.position = positionCS;
    o.positionSS = ComputeScreenPos(positionCS);
    o.shadowCoord = TransformWorldToShadowCoord(positionWS);
    return o;
}

float4 frag(v2f i) : SV_Target
{   
#if DEPTH_ONLY
    return 0;
#endif

    float4 waterColor = 0;
    float4 shadowCoord = i.shadowCoord;
    float2 uvSS = i.positionSS.xy / i.positionSS.w;
    {
        float sceneEyeDepth = SampleSceneEyeDepth(uvSS);
        float fragEyeDepth = CalculateFragEyeDepth(i.position);
        float waterDepth = sceneEyeDepth - fragEyeDepth;
        float4 shallowWaterColor = _ShallowWaterColor;
        float4 deepWaterColor = _DeepWaterColor;
        float shallowWaterDepth = _ShallowWaterDepth;
        float deepWaterDepth = _DeepWaterDepth;
        float waterVisibility = _WaterVisibility;
        float depthProgress = saturate((waterDepth - shallowWaterDepth) / (deepWaterDepth - shallowWaterDepth));
        waterColor = lerp(
            shallowWaterColor,
            deepWaterColor,
            depthProgress
        );
        float2 noiseUV = i.uv * 5 + float2(1, 0) * _Time.x * 4;
        float2 sceneNoise = snoise(float3(noiseUV, 0)) * float2(0.05, 0.00);
        float3 sceneColor = SampleSceneColor(uvSS + sceneNoise * saturate(waterDepth));
        float sceneColorAlpha = LinearStep(
            lerp(shallowWaterDepth, deepWaterDepth , waterVisibility),
            0,
            waterDepth);
        waterColor.xyz = lerp(waterColor.xyz, sceneColor, sceneColorAlpha);
        shadowCoord.xy += sceneNoise * 0.04;
    }
    
    {
        float scaleX = _WaveNoiseUVScaleX;
        float scaleY = _WaveNoiseUVScaleY;
        float speedZ = _WaveNoiseUVSpeedZ;
        float cutoff = _WaveNoiseCutoff;
        float4 waveColor = _WaveColor;
        
        float time = GetTime(0.05);
        
        float3 noiseUV = float3(
            i.uv.x * scaleX,
            i.uv.y * scaleY,
            time * speedZ
        );
        float noise = snoise(noiseUV) * .5 + .5;
        float wave = step(cutoff, noise);
        waterColor.xyz = lerp(waterColor.xyz, waveColor.rgb, wave);
    }
    
    float3 normal = float3(0, 1, 0);
    
    int2 pxielPos =  CalculatePixelPos(i.position.xy, 0);
    float2 lightingIntensity = float2(1, 0);
    float ao = 1;
    float smoothness = 100000;
    //waterColor = ApplyWaterPixelizeLighting(waterColor, normal, pxielPos, i.positionWS, shadowCoord);
    waterColor = ApplyPixelizeLighting(waterColor, normal, pxielPos, i.positionWS, shadowCoord, smoothness, lightingIntensity, ao);
    //ssao
    //#if SCREEN_SPACE_OCCLUSION
    {
        waterColor.rgb = ApplySSAO(waterColor.rgb, uvSS);
    }
    //#endif

    return waterColor;
}


#endif