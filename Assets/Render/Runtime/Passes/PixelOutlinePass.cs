using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
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
        private RenderTargetHandle outlineDepthTexture;
        private ShaderTagId shaderTag;
        private ScriptableRenderer renderer;
        public ScriptableRenderer Renderer { set => renderer = value;}
        
        public PixelOutlinePass(PixelOutline _setting)
        {
            setting = _setting;
            outlineTex.Init("_OutlineTex");
            outlineDepthTexture.Init("_OutlineDepthTexture");
            shaderTag = new ShaderTagId("PixelOutline");
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            var des = cameraTextureDescriptor;
            des.depthBufferBits = 0;
            des.colorFormat = RenderTextureFormat.ARGBHalf;
            //des.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R32G32_UInt;
            cmd.GetTemporaryRT(outlineTex.id, des, FilterMode.Point);

            var depthDes = cameraTextureDescriptor;
            depthDes.msaaSamples = 1;
            depthDes.colorFormat = RenderTextureFormat.Depth;
            depthDes.depthBufferBits = 32;
            depthDes.sRGB = false;
            depthDes.useMipMap = false;
            depthDes.autoGenerateMips = false;
            cmd.GetTemporaryRT(outlineDepthTexture.id, depthDes, FilterMode.Point);
        }       

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(setting.name);

            //draw outline pass
            var colorTex = renderer.cameraColorTarget;
            var depthTex = renderer.cameraDepthTarget;
            cmd.SetRenderTarget(outlineTex.Identifier(), outlineDepthTexture.Identifier());
            cmd.ClearRenderTarget(true, true, new Color(0, 0, 0, 0));
            var desc = new RendererListDesc(shaderTag, renderingData.cullResults, renderingData.cameraData.camera);
            desc.renderQueueRange = RenderQueueRange.all;
            desc.sortingCriteria = SortingCriteria.CommonOpaque;
            var rendererList = context.CreateRendererList(desc);
            cmd.DrawRendererList(rendererList);

            //copydepth
            // cmd.SetRenderTarget(new RenderTargetIdentifier(tempDepthTex.Identifier(), 0, CubemapFace.Unknown, -1), tempDepthTex.id);
            // cmd.SetGlobalTexture(ShaderConstant._DepthTex, depthTex);
            // cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, setting.OutlineMaterial, 0, 0);
            
            cmd.SetRenderTarget(colorTex, depthTex);
            cmd.SetGlobalTexture(ShaderConstant._DepthTex, outlineDepthTexture.id);
            cmd.SetGlobalTexture(outlineTex.id, outlineTex.Identifier());
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, setting.OutlineMaterial, 0, 1);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            cmd.ReleaseTemporaryRT(outlineTex.id);
            cmd.ReleaseTemporaryRT(outlineDepthTexture.id);
        }
    }
}

