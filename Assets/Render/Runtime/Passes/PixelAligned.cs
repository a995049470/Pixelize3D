using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace Lost.Render.Runtime
{

    public class PixelAligned : ScriptableRendererFeature
    {
        // [SerializeField][Range(1, 1080)]
        // private float targetRenderTextureHeight = 1080;
        [SerializeField][Range(1, 8)]
        private int pixelSize = 1;
        [SerializeField][Range(1, 8)]
        private int shadowPixelSize = 1;
        [SerializeField]
        private bool cameraAligned = false;
        [SerializeField]
        private bool discardPixel = false;
        [SerializeField]
        private bool softParticles = false;
        [SerializeField][Range(0, 1)]
        private float minLDotN = 0;
        [SerializeField][Range(0, 1f)]
        private float ditherWidth = 0;
        [SerializeField][Range(0, 2f)]
        private float aoDitherWidth = 0;
        [SerializeField]
        private bool useLightLevel = true;
        [SerializeField][Range(1, 8)]
        private float lightLevel = 1;
        
        //public bool PixelizeBRDF = false;
        [SerializeField]
        private float ditherPower = 1;
        [SerializeField][Range(0, 4)]
        private float diffuseIntensity = 1;
        [SerializeField][Range(0, 16)]
        private float specularIntensity = 1;
        [ColorUsage(true, true)]
        public Color PixelAmbientLight;

        public int PixelSize { get => pixelSize; }
        public int ShadowPixelSize { get => shadowPixelSize; }
        public float MinLDotN { get => minLDotN; }
        public float DitherWidth { get => ditherWidth; }
        public float AODitherWidth { get => aoDitherWidth; }
        public float LightLevel { get => useLightLevel ? lightLevel : 65536; }
        public int DiscardPixel { get => discardPixel ? 1 : 0; }
        public bool SoftParticles { get => softParticles;}
        public bool CameraAligned { get => cameraAligned; }
        public Vector4 PxielLightingParameter { get => new Vector4(diffuseIntensity, specularIntensity, ditherPower); }
        private PixelAlignedPass pass;

        public int GetPixelSize(int renderTextureHeight)
        {
            return pixelSize;
            // float pixelSize = (float)renderTextureHeight / targetRenderTextureHeight;
            // return Mathf.Clamp(
            //     Mathf.RoundToInt(pixelSize),
            //     1,
            //     65536
            // );
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            //RenderPassSharedData.Instance.SetInt(ShaderConstant._PixelSize, pixelSize);
            renderer.EnqueuePass(pass);
        }

        public override void Create()
        {
            pass = pass ?? new PixelAlignedPass(this);
        }

        
    }
}

