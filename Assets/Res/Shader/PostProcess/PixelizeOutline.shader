Shader "Lost/Pixelize Outline"
{
    Properties
    {
        
        [Main(InsideOutline, _INSIDE_OUTLINE)]
        _InsideOutline ("InsideOutline", float) = 0
        [Sub(InsideOutline)]
        _MinDepthOffset ("MinDepthOffset", Range(0, 1)) = 0.5
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Assets/Res/Shader/Common/AlignedPixel.hlsl"
    #include "Assets/Res/Shader/PostProcess/FullScreenTriangle.hlsl"

    #define INSIDE_OUTLINE _INSIDE_OUTLINE

    struct appdata
    {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f
    {
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    
    Texture2D<float4> _OutlineTex;
    float4 _OutlineTex_TexelSize;
    float _MinDepthOffset;
    

    TEXTURE2D_X_FLOAT(_DepthTex);
    SAMPLER(sampler_DepthTex);

    

    v2f vert(appdata v)
    {
        v2f o = (v2f)0;
        o.position = v.positionOS;
        o.uv = TransformTriangleVertexToUV(v.positionOS.xy);
        return o;
    }

    float frag_copy_depth(v2f i) : SV_DEPTH
    {
        return _DepthTex.Load(int3(i.position.xy, 0)).r;
    }
    

    // float OrthoLinearEyeDepth(float depth)
    // {
    //     #if UNITY_REVERSED_Z
    //         depth = 1 - depth;
    //     #endif
    //     return lerp(_ProjectionParams.y, _ProjectionParams.z, depth);
    // }

    float4 frag(v2f i, out float fragDepth : SV_DEPTH) : SV_Target
    {
        float4 color = 0;
        
        int2 midPos = int2(i.position.xy);
        // midoutline == w
        float4 midOutline = _OutlineTex.Load(int3(midPos, 0));
        DiscardPixel(midPos, 0);
        //if(midOutline.w == 0) discard;
       
        float midDepth = _DepthTex.Load(int3(midPos, 0)).x;
    #if INSIDE_OUTLINE
        float midEyeDepth = OrthoLinearEyeDepth(midDepth);
    #endif
        bool isRead = false;
        for (int i = 0; i < 9; i++)
        {
            int x = (i % 3 - 1) * _PixelSize;
            int y = (i / 3 - 1) * _PixelSize;
            int2 pos = midPos + int2(x, y);
            float4 outline = _OutlineTex.Load(int3(pos, 0));
            float depth = _DepthTex.Load(int3(pos, 0)).x;
        #if INSIDE_OUTLINE
            float eyeDepth = OrthoLinearEyeDepth(depth);
            #if UNITY_REVERSED_Z
            if ((outline.w != 0) && ((outline.w != midOutline.w && depth > midDepth) || (midEyeDepth - eyeDepth > _MinDepthOffset)))
            #else
            if ((outline.w != 0) && ((outline.w != midOutline.w && depth < midDepth) || (midEyeDepth - eyeDepth > _MinDepthOffset)))
            #endif
        #else
            #if UNITY_REVERSED_Z
            if ((outline.w != 0) && (outline.w != midOutline.w && depth > midDepth)) 
            #else
            if ((outline.w != 0) && (outline.w != midOutline.w && depth < midDepth)) 
            #endif
        #endif
            {
                isRead = isRead || i != 4;
                midDepth = depth;
                color.rgb = outline.rgb;
                color.a = 1;
            }
        }
        
        if(!isRead)
        {
            discard;
        }
        fragDepth = midDepth;
    
        return color;
    }

    ENDHLSL
    
    SubShader
    {
        
        Pass
        {
            Name "CopyDpeth"
            Blend One Zero
            ZWrite On
            ZTest Always
            Cull Off
            ColorMask 0
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag_copy_depth
            ENDHLSL
        }

        Pass
        {
            Name "Outline"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ZTest Always
            Cull Off
            
            Stencil
            {
                Ref 1
                ReadMask 1
                WriteMask 1
                Comp NotEqual
                Pass Replace
            }

            HLSLPROGRAM
            #pragma multi_compile __ _INSIDE_OUTLINE
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }

    }
    CustomEditor "LWGUI.LWGUI"
}
