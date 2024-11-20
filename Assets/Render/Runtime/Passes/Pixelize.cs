using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace Lost.Render.Runtime
{

    public class Pixelize : ScriptableRendererFeature
    {
        [SerializeField] Material pixelizeMaterial;

        private PixelizePass pass;

        public Material PixelizeMaterial { get => pixelizeMaterial; }
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if( pixelizeMaterial != null && pixelizeMaterial.passCount >= 2)
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

 