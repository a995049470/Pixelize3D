using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace Lost.Render.Runtime
{

    public class PixelAligned : ScriptableRendererFeature
    {
        [SerializeField][Range(1, 8)]
        private int pixelSize = 1;
        [SerializeField][Range(1, 8)]
        private int shadowPixelSize = 1;
        [SerializeField][Range(0, 1)]
        private float minLDotN = 0;
        [SerializeField][Range(0, 1f)]
        private float ditherWidth = 0;
        [SerializeField]
        private bool useLightLevel = true;
        [SerializeField][Range(1, 8)]
        private int lightLevel = 1;

        public int PixelSize { get => pixelSize; }
        public int ShadowPixelSize { get => shadowPixelSize; }
        public float MinLDotN { get => minLDotN; }
        public float DitherWidth { get => ditherWidth; }
        public int LightLevel { get => useLightLevel ? lightLevel : 65536; }
        private PixelAlignedPass pass;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            RenderPassSharedData.Instance.SetInt(ShaderConstant._PixelSize, pixelSize);
            renderer.EnqueuePass(pass);
        }

        public override void Create()
        {
            pass = pass ?? new PixelAlignedPass(this);
        }

        
    }
}

