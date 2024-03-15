using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lost.Render.Runtime
{
    public class PixelOutline : ScriptableRendererFeature
    {
        [SerializeField]
        private Material outlineMaterial;
        public Material OutlineMaterial { get => outlineMaterial; }
        private PixelOutlinePass pass;
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if(outlineMaterial != null)
            {
                pass.Renderer = renderer;
                renderer.EnqueuePass(pass);
            }
        }

        public override void Create()
        {
            pass = pass ?? new PixelOutlinePass(this);
        }
    }
}

