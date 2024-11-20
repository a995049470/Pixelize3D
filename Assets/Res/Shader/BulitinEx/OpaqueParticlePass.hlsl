#ifndef OPAQUE_PARTICLE_PASS
#define OPAQUE_PARTICLE_PASS

#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_particles
//#pragma multi_compile_fog

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages\com.unity.render-pipelines.core\ShaderLibrary\Color.hlsl"
#include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Assets/Res/Shader/Common/CGMacro.hlsl"
#include "Assets\Res\Shader\Common\Dither.hlsl"

#define SOFTPARTICLES_ON 0

#define OUTLINE _OUTLINE

sampler2D _MainTex;
fixed4 _TintColor;

#define SCALE_V 0.03

struct appdata_t
{
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    
    #if SOFTPARTICLES_ON
        float4 projPos : TEXCOORD2;
    #endif
    #if OUTLINE
    float originPos             : TEXCOORD3;
    #endif
    UNITY_VERTEX_OUTPUT_STEREO
};

float4 _MainTex_ST;
float _UseCustomObjectPosition;
float3 _ObjectPosition;

v2f vert(appdata_t v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    float3 objectPositon = lerp(GetObjectPosition(), _ObjectPosition, _UseCustomObjectPosition);
    AlignedPixelOutput result = TransformObjectToHClip_AlignedPixel(v.vertex.xyz, objectPositon);
    o.vertex = result.positionCS;
    #if SOFTPARTICLES_ON
        o.projPos = ComputeScreenPos(o.vertex);
        //COMPUTE_EYEDEPTH(o.projPos.z);
    #endif
    o.color = v.color * _TintColor;
    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    #if OUTLINE
    float3 originPosWS = result.originPosWS;
    o.originPos = originPosWS.x * 17.176 + originPosWS.y * 123.131 - originPosWS.z * 319.7931;
    #endif
    //UNITY_TRANSFER_FOG(o,o.vertex);
    return o;
}

//UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
float _InvFade;

fixed4 frag(v2f i) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);


    #if SOFTPARTICLES_ON
        float depth = SampleSceneDepth(i.projPos.xy / i.projPos.w);
        float sceneZ = unity_OrthoParams.w == 0 ? LinearEyeDepth(depth, _ZBufferParams) : LinearDepthToEyeDepth(depth);
        float partZ = unity_OrthoParams.w == 0 ? i.vertex.w : LinearDepthToEyeDepth(i.vertex.z);
        float fade = saturate(_InvFade * (sceneZ - partZ));
        i.color.a *= fade;
    #endif

    fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
    col.a = saturate(col.a); // alpha should not have double-brightness applied to it, but we can't fix that legacy behavior without breaking everyone's effects, so instead clamp the output to get sensible HDR behavior (case 967476)
   
    #if OUTLINE
        float3 outlineColor = col.rgb;
        float3 hsv = RgbToHsv(outlineColor);
        hsv.z *= SCALE_V;
        outlineColor = HsvToRgb(hsv);
        col = float4(outlineColor, i.originPos);
    #else
        DiscardPixel(int2(i.vertex.xy), 0);
        Unity_Dither_Pixel(col.a, int2(i.vertex.xy), 1);
    #endif
    ////UNITY_APPLY_FOG(i.fogCoord, col);
    return col;
}
#endif