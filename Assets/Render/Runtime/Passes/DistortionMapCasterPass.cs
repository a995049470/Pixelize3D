using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;

namespace Lost.Render.Runtime
{
    public class DistortionMapCasterPass : ScriptableRenderPass
    {
        private DistortionMapCaster setting;
        private RenderTargetHandle distortionMap;
        private RenderTargetHandle distortionDepthTex;
        private ShaderTagId shaderTagId;
        
        public DistortionMapCasterPass(DistortionMapCaster _setting)
        {
            setting = _setting;
            shaderTagId = new ShaderTagId("Distortion");
            distortionMap.Init("_DistortionMap");
            distortionDepthTex.Init("_DistortionDepthTex");
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);

            var des = cameraTextureDescriptor;
            des.depthBufferBits = 0;
            des.colorFormat = RenderTextureFormat.RGHalf;
            des.width = Mathf.Max(1, des.width / setting.DownSampleCount);
            des.height = Mathf.Max(1, des.height / setting.DownSampleCount);
            cmd.GetTemporaryRT(distortionMap.id, des, FilterMode.Point);

            var depthDes = cameraTextureDescriptor;
            depthDes.msaaSamples = 1;
            depthDes.colorFormat = RenderTextureFormat.Depth;
            depthDes.depthBufferBits = 32;
            depthDes.sRGB = false;
            depthDes.width = Mathf.Max(1, des.width / setting.DownSampleCount);
            depthDes.height = Mathf.Max(1, des.height / setting.DownSampleCount);
            cmd.GetTemporaryRT(distortionDepthTex.id, depthDes, FilterMode.Point);
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(setting.name);
            var depthTex = setting.Renderer.cameraDepthTarget;

            cmd.EnableShaderKeyword(ShaderConstant._PIXELIZE_DISTORTION);
            //拷贝深度
            cmd.SetRenderTarget(new RenderTargetIdentifier(distortionDepthTex.Identifier(), 0, CubemapFace.Unknown, -1), distortionDepthTex.id);
            cmd.SetGlobalTexture(ShaderConstant._DepthTex, depthTex);
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, setting.CopyDepthMaterial, 0, 0);
            
            //绘制扰动图
            cmd.SetRenderTarget(distortionMap.id, distortionDepthTex.id);
            cmd.ClearRenderTarget(false, true, Color.black);
            var des = new RendererListDesc(shaderTagId, renderingData.cullResults, renderingData.cameraData.camera)
            {
                renderQueueRange = RenderQueueRange.all,
                layerMask = -1,
                sortingCriteria = SortingCriteria.None
            };
            var renderList = context.CreateRendererList(des);
            cmd.DrawRendererList(renderList);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            cmd.ReleaseTemporaryRT(distortionMap.id);
            cmd.ReleaseTemporaryRT(distortionDepthTex.id);
            cmd.DisableShaderKeyword(ShaderConstant._PIXELIZE_DISTORTION);
        }
    }
}

