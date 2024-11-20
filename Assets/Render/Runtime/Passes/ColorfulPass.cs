using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

namespace Lost.Render.Runtime
{
    public class ColorfulPass : ScriptableRenderPass
    {
        private Colorful setting;
        private RenderTargetHandle colorfulTex;
        private ScriptableRenderer renderer;
        public ScriptableRenderer Renderer { set => renderer = value;}

        public ColorfulPass(Colorful _setting)
        {
            setting = _setting;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents + 1;
            colorfulTex.Init("_ColorfulTex");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            cmd.GetTemporaryRT(colorfulTex.id, cameraTextureDescriptor, FilterMode.Point);
            ConfigureTarget(colorfulTex.Identifier());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var colorAttachment = renderer.cameraColorTarget;
            var cmd = CommandBufferPool.Get(setting.name);
            var quad = RenderingUtils.fullscreenMesh;
            cmd.SetGlobalTexture(ShaderConstant._MainTex, colorAttachment);
            cmd.SetRenderTarget(colorfulTex.id);
            cmd.DrawMesh(quad, Matrix4x4.identity, setting.ColorfulMat, 0, 0);
            cmd.SetGlobalTexture(ShaderConstant._MainTex, colorfulTex.id);
            cmd.SetRenderTarget(colorAttachment);
            cmd.DrawMesh(quad, Matrix4x4.identity, setting.ColorfulMat, 0, 1);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            cmd.ReleaseTemporaryRT(colorfulTex.id);
        }
    }
}

