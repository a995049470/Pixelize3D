#ifndef LPIPELINE_LIGHT_POINT_PASS
#define LPIPELINE_LIGHT_POINT_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#include "Common.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 texcoord : TEXCOORD;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : POSITION;
    float4 positionSS : TEXCOORD0;
    float2 uv : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Texture2D _DepthTexture; SamplerState sampler_DepthTexture;
Texture2D _GBuffer0; SamplerState sampler_GBuffer0;
Texture2D _GBuffer1; SamplerState sampler_GBuffer1;
Texture2D _GBuffer2; SamplerState sampler_GBuffer2;
//TextureCube _LightMask; SamplerState sampler_LightMask;

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _LightParameter)
    UNITY_DEFINE_INSTANCED_PROP(float3, _LightColor)
    UNITY_DEFINE_INSTANCED_PROP(float, _Diffuse)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

float4x4 _InvVP;
float _IntensityBias;

#include "BRDF.hlsl"
#include "MetaCellGI.hlsl"

Varyings Vertex(Attributes i)
{
    Varyings o = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
    o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
    o.positionSS = ComputeScreenPos(o.positionCS);
    o.uv = i.texcoord;
    return o;
}



float GetLightIntensity(float dis)
{
    float4 lightParameter = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _LightParameter);
    float intensity = 1.0 / (lightParameter.y * dis * dis + lightParameter.z * dis + lightParameter.w);
    intensity = max(intensity, _IntensityBias);
    intensity = (intensity - _IntensityBias) / (1.0 - _IntensityBias) * lightParameter.x;
    
    return intensity; 
}

float2 PositionToUV(float3 position)
{
    float r = sqrt(position.x * position.x + position.y * position.y + position.z * position.z);
    float u = atan(position.y / position.x) / 2.000f / PI;
    float v = asin(position.z / r) / PI + 0.500f;
    return float2(u, v);
}

float4 Fragment(Varyings i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    float2 uvSS = i.positionSS.xy / i.positionSS.w;
    float depth = _DepthTexture.Sample(sampler_DepthTexture, uvSS).r;
    float3 albedo = _GBuffer0.Sample(sampler_GBuffer0, uvSS).rgb;
    float3 normalWS = normalize(_GBuffer1.Sample(sampler_GBuffer1, uvSS).rgb);
    float3 positionWS = ScreenPosToWorldPosition(uvSS, depth, _InvVP);
    float3 lightPosition = float3(UNITY_MATRIX_M[0][3], UNITY_MATRIX_M[1][3], UNITY_MATRIX_M[2][3]);
    float3 lightDir = normalize(lightPosition - positionWS);
    float dis = distance(positionWS, lightPosition);
    float intensity = GetLightIntensity(dis);
    
    float ldotn = dot(normalWS, lightDir);
    ldotn = max(0, ldotn);
    float3 viewDir = normalize(GetCameraPositionWS() - positionWS);

    float3 gbuffer2 = _GBuffer2.Sample(sampler_GBuffer2, uvSS);
    float metallic = gbuffer2.x;
    float roughness = gbuffer2.y;
    float ao = gbuffer2.z;

    float3 uv_mask = normalize(TransformWorldToObject(positionWS));

    float3 mask = 1;
    float3 lightColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _LightColor);
    float3 finalLightColor = lightColor * intensity * mask;
    float diffuse = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Diffuse);
    float2 strength = float2(diffuse, 1);
    float3 brdf = BDRF(lightDir, viewDir, normalWS, albedo, finalLightColor, roughness, metallic, ao, strength);
    float3 visiable_gi = GetMetaCellGIVisiable(positionWS, normalWS);
    
    float3 color = brdf * visiable_gi;
    
    return float4(color, 1);
}



#endif