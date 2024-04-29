Shader "LPipeline/Lit2D_Point"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity("Intensity", Range(0, 24)) = 1
    }
    SubShader
    {

        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Transparent"
        }
        Pass
        {
            Tags
            {
                "LightMode"="Light2D"
            }
            Blend One One
            ZWrite Off
            ZTest Always
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Texture2D _MainTex;
            SamplerState sampler_MainTex;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.uv = v.uv;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.color = float4(v.color.rgb * (pow(2, _Intensity) - 1), v.color.a);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = _MainTex.Sample(sampler_MainTex, i.uv);
                col *= i.color;
                return col;
            }
            ENDHLSL
        }
    }
}
