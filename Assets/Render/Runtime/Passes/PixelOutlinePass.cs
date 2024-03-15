using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;

namespace Lost.Render.Runtime
{
    public class PixelOutlinePass : ScriptableRenderPass
    {
        private PixelOutline setting;
        private RenderTargetHandle outlineTex;
        private ShaderTagId shaderTag;
        private ScriptableRenderer renderer;
        public ScriptableRenderer Renderer { set => renderer = value;}
        
        public PixelOutlinePass(PixelOutline _setting)
        {
            setting = _setting;
            outlineTex.Init("_OutlineTex");
            shaderTag = new ShaderTagId("PixelOutline");
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            var des = cameraTextureDescriptor;
            des.depthBufferBits = 0;
            des.colorFormat = RenderTextureFormat.ARGBHalf;
            //des.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R32G32_UInt;
            cmd.GetTemporaryRT(outlineTex.id, des, FilterMode.Point);
        }       

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(setting.name);
            var colorTex = renderer.cameraColorTarget;
            var depthTex = renderer.cameraDepthTarget;
            cmd.SetRenderTarget(outlineTex.Identifier(), depthTex);
            cmd.ClearRenderTarget(false, true, new Color(0, 0, 0, 0));
            var desc = new RendererListDesc(shaderTag, renderingData.cullResults, renderingData.cameraData.camera);
            desc.renderQueueRange = RenderQueueRange.opaque;
            desc.sortingCriteria = SortingCriteria.CommonOpaque;
            var rendererList = context.CreateRendererList(desc);
            cmd.DrawRendererList(rendererList);
            cmd.SetRenderTarget(colorTex);
            cmd.SetGlobalTexture(ShaderConstant._DepthTex, depthTex);
            cmd.SetGlobalTexture(outlineTex.id, outlineTex.Identifier());
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, setting.OutlineMaterial);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            cmd.ReleaseTemporaryRT(outlineTex.id);
        }
    }
}

