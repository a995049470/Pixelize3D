Shader "LPipeline/VolumeFog"
{
    Properties
    {
        _Color("Color", color) = (1, 1, 1, 1)
    }
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    float4 _Color;
    struct appdata
    {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
        float3 normalOS : NORMAL;
    };

    struct v2f_scene_view
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS :TEXCOORD0;
        float3 normalOS : TEXCOORD1;    
    };

    struct v2f_back
    {
        float4 positionCS : SV_POSITION;
    };

    struct v2f_front
    {
        float4 positionCS : SV_POSITION;
    };

    struct v2f_fog
    {
        float4 positionCS : SV_POSITION;
        float4 uvSS : TEXCOORD0;
    };
    
    v2f_scene_view vert_scene_view(appdata i)
    {
        v2f_scene_view o = (v2f_scene_view)0;
        o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
        o.positionWS = TransformObjectToWorld(i.positionOS.xyz);
        o.normalOS = i.normalOS;
        return o;
    }

    // v2f_back vert_back(appdata i)
    // {

    // }

    float4 frag_scene_view(v2f_scene_view i) : SV_TARGET
    {
        float3 normal = i.normalOS;
        float3 up = float3(0, 1, 0);
        float VDotU = dot(normal, up) * .5 + .5;
        float4 color = _Color * VDotU;
        return color;
    }


    ENDHLSL

    SubShader
    {

        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }
        Pass
        {
            ZWrite On
            ZTest Less
            HLSLPROGRAM
            #pragma vertex vert_scene_view
            #pragma fragment frag_scene_view

            
            
            ENDHLSL
        }
    }
}
