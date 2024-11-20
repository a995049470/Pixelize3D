using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lost.Render.Runtime
{
    public class Colorful : ScriptableRendererFeature
    {
        public Material ColorfulMat;
        private ColorfulPass pass;

        private bool IsVaild()
        {
            return ColorfulMat != null && ColorfulMat.passCount >= 2;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if(IsVaild())
            {
                pass.Renderer = renderer;
                renderer.EnqueuePass(pass);
            }
        }

        public override void Create()
        {
            pass = pass ?? new(this);
        }

        
    }
}

