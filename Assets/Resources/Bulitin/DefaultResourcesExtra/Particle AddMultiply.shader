// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Lost/Legacy Shaders/Particles/~Additive-Multiply" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend One OneMinusSrcAlpha
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off

    SubShader {
        Pass {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            //#pragma multi_compile_fog

            #include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Assets/Res/Shader/Common/CGMacro.hlsl"

            sampler2D _MainTex;
            fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                ////UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                AlignedPixelOutput res = TransformObjectToHClip_AlignedPixel(v.vertex.xyz, GetObjectPosition());
                o.vertex = res.positionCS;
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                ////COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                ////UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            ////UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                #ifdef SOFTPARTICLES_ON
                float depth = SampleSceneDepth(i.projPos.xy / i.projPos.w);
                float sceneZ = unity_OrthoParams.w == 0 ? LinearEyeDepth(depth, _ZBufferParams) : LinearDepthToEyeDepth(depth);
                float partZ = unity_OrthoParams.w == 0 ? i.vertex.w : LinearDepthToEyeDepth(i.vertex.z);
                float fade = saturate(_InvFade * (sceneZ - partZ));
                i.color.a *= fade;
                #endif

                fixed4 tex = tex2D(_MainTex, i.texcoord);
                fixed4 col;
                col.rgb = _TintColor.rgb * tex.rgb * i.color.rgb * 2.0f;
                col.a = (1 - tex.a) * (_TintColor.a * i.color.a * 2.0f);
                ////UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
                return col;
            }
            ENDHLSL
        }
    }

}
}
