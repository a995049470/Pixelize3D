Shader "LPipeline/Lit2DUpdate"
{
    Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius("BlurRadius", Range(0, 8)) = 1
        _SideWeight("SideWeight", Range(0, 16)) = 1
        _MidWeight("MidWeight", Range(0, 16)) = 1
        _DiagWeight("DiagWeight", Range(0, 16)) = 1
    }
    SubShader
    {

        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }
        Pass
        {
            Tags
            {
                "LightMode"="LPipeline"
            }
            Blend One Zero
            ZWrite On
            ZTest Less
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 uv12 : TEXCOORD1;
                float4 uv34 : TEXCOORD2;
                float4 uv56 : TEXCOORD3;
                float4 uv78 : TEXCOORD4;
            };

            Texture2D _MainTex;
            SamplerState sampler_MainTex;
            float4 _MainTex_TexelSize;
            float _BlurRadius;
            float _MidWeight;
            float _SideWeight;
            float _DiagWeight;

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.uv = v.uv;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                float4 offset = _MainTex_TexelSize.xyxy * _BlurRadius;
                o.uv12 = o.uv.xyxy + offset * float4(1, 0, -1, 0);
                o.uv34 = o.uv.xyxy + offset * float4(0, 1, 0, -1);
                o.uv56 = o.uv.xyxy + offset * float4(1, 1, 1, -1);
                o.uv78 = o.uv.xyxy + offset * float4(-1, 1, -1, -1);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = _MainTex.Sample(sampler_MainTex, i.uv);
                float4 c1 = _MainTex.Sample(sampler_MainTex, i.uv12.xy);
                float4 c2 = _MainTex.Sample(sampler_MainTex, i.uv12.zw);
                float4 c3 = _MainTex.Sample(sampler_MainTex, i.uv34.xy);
                float4 c4 = _MainTex.Sample(sampler_MainTex, i.uv34.zw);
                float4 c5 = _MainTex.Sample(sampler_MainTex, i.uv56.xy);
                float4 c6 = _MainTex.Sample(sampler_MainTex, i.uv56.zw);
                float4 c7 = _MainTex.Sample(sampler_MainTex, i.uv78.xy);
                float4 c8 = _MainTex.Sample(sampler_MainTex, i.uv78.zw);
                float4 c = (col * _MidWeight + c1 * _SideWeight + c2 * _SideWeight + c3 * _SideWeight + c4 * _SideWeight + c5 * _DiagWeight + c6 * _DiagWeight + c7 * _DiagWeight + c8 * _DiagWeight) / (_SideWeight * 4 + _DiagWeight * 4 + _MidWeight);
                col = max(col, c);
                return col;
            }
            ENDHLSL
        }
    }
}
