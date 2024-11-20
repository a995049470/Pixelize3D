using UnityEngine;

namespace Lost.Render.Runtime
{

    public static class ShaderConstant
    {
        public readonly static int _MainTex = Shader.PropertyToID("_MainTex");
        public readonly static int _DepthTex = Shader.PropertyToID("_DepthTex");

        public static readonly int _PixelSize = Shader.PropertyToID("_PixelSize");
        public static readonly int _RenderTarget_TexelSize = Shader.PropertyToID("_RenderTarget_TexelSize");
        public static readonly int _CameraOffsetVS = Shader.PropertyToID("_CameraOffsetVS");
        public static readonly int _CameraOffsetSS = Shader.PropertyToID("_CameraOffsetSS");
        public static readonly int _PixelSizeCS = Shader.PropertyToID("_PixelSizeCS");
        public static readonly int _LightLevel = Shader.PropertyToID("_LightLevel");
        public static readonly int _MinLDotN = Shader.PropertyToID("_MinLDotN");
        public static readonly int _DitherWidth = Shader.PropertyToID("_DitherWidth");
        public static readonly int _AODitherWidth = Shader.PropertyToID("_AODiterWidth");
        public static readonly int _MatrixV_AlignedPixel = Shader.PropertyToID("_MatrixV_AlignedPixel");
        public static readonly int _MatrixInvV_AlignedPixel = Shader.PropertyToID("_MatrixInvV_AlignedPixel");
        public static readonly int _MatrixVP_AlignedPixel = Shader.PropertyToID("_MatrixVP_AlignedPixel");
        public static readonly int _MatrixInvVP_AlignedPixel = Shader.PropertyToID("_MatrixInvVP_AlignedPixel");
        public static readonly int _RenderScale = Shader.PropertyToID("_RenderScale");
        public static readonly int _ShadowPixelSize = Shader.PropertyToID("_ShadowPixelSize");
        public static readonly int _RelativePosition = Shader.PropertyToID("_RelativePosition");
        public static readonly int _DiscardPixel = Shader.PropertyToID("_DiscardPixel");
        public static readonly int _HitColor = Shader.PropertyToID("_HitColor");
        public static readonly int _PixelLightingParameter = Shader.PropertyToID("_PixelLightingParameter");
        public static readonly int _PixelAmbientLight = Shader.PropertyToID("_PixelAmbientLight");
        public static readonly int _ObjectPosition = Shader.PropertyToID("_ObjectPosition");
        public static readonly int _MainTexRotate  = Shader.PropertyToID("_MainTexRotate");

        public const string _PIXELATE = "_PIXELATE";
        public const string _SMOOTH_UPSAMPLE = "_SMOOTH_UPSAMPLE";
        public const string SOFTPARTICLES_ON = "SOFTPARTICLES_ON";
        public const string _PIXELIZE_BRDF = "_PIXELIZE_BRDF";
        public const string _PIXELIZE_DISTORTION = "_PIXELIZE_DISTORTION";

        //--------------------临时字段-----------------------------------
        public static readonly int _Temp0 = Shader.PropertyToID("_Temp0");
        public static readonly int _Temp1 = Shader.PropertyToID("_Temp1");
        public static readonly int _Temp2 = Shader.PropertyToID("_Temp2");
        public static readonly int _Temp3 = Shader.PropertyToID("_Temp3");

    }
}