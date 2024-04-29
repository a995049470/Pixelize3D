Shader "LPipeline/Lit2DBarrier"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {

        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Pass
        {
            Tags
            {
                "LightMode"="LightBarrier"
            }

            ZWrite Off
            ZTest Less
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                //rgb 代表屏障的颜色, a 代表光通率
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            Texture2D _MainTex;
            SamplerState sampler_MainTex;

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.uv = v.uv;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.color = v.color;
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
