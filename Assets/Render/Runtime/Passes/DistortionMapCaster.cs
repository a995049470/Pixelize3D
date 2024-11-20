using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lost.Render.Runtime
{
    public class DistortionMapCaster : ScriptableRendererFeature
    {
        [Range(1, 8)]
        public int DownSampleCount = 1;
        public Material CopyDepthMaterial;
        public ScriptableRenderer Renderer { get; private set; }
        private DistortionMapCasterPass pass;
        public bool IsVaild()
        {
            return CopyDepthMaterial != null;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Renderer = renderer;
            if(IsVaild())
            {
                renderer.EnqueuePass(pass);
            }
        }

        public override void Create()
        {
            pass = pass ?? new(this);
        }

      
        
    }
}

