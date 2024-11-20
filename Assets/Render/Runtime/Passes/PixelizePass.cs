using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

namespace Lost.Render.Runtime
{

    public class PixelizePass : ScriptableRenderPass
    {
        private Pixelize setting;
        private RenderTargetHandle pixelizeColorTexture;
        private ScriptableRenderer renderer;
        public ScriptableRenderer Renderer { set => renderer = value;}

        public PixelizePass(Pixelize _setting)
        {
            setting = _setting;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            pixelizeColorTexture.Init("_PixelizeColorTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            cmd.GetTemporaryRT(pixelizeColorTexture.id, cameraTextureDescriptor, FilterMode.Bilinear);
            ConfigureTarget(pixelizeColorTexture.Identifier());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var colorAttachment = renderer.cameraColorTarget;
            var depthAttachment = renderer.cameraDepthTarget;
            // ref CameraData cameraData = ref renderingData.cameraData;
            // RenderTargetIdentifier cameraTarget = (cameraData.targetTexture != null) ? new RenderTargetIdentifier(cameraData.targetTexture) : BuiltinRenderTextureType.CameraTarget;
            
            var cmd = CommandBufferPool.Get(setting.name);
            cmd.Clear();
            var quad = RenderingUtils.fullscreenMesh;
            cmd.SetGlobalTexture(ShaderConstant._MainTex, colorAttachment);
            cmd.SetGlobalTexture(ShaderConstant._DepthTex, depthAttachment);
            cmd.DrawMesh(quad, Matrix4x4.identity, setting.PixelizeMaterial, 0, 0);

            cmd.SetRenderTarget(colorAttachment);
            cmd.SetGlobalTexture(ShaderConstant._MainTex, pixelizeColorTexture.id);
            cmd.DrawMesh(quad, Matrix4x4.identity, setting.PixelizeMaterial, 0, 1);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            cmd.ReleaseTemporaryRT(pixelizeColorTexture.id);
        }
    }
}

