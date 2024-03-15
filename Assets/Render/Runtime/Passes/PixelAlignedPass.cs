using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Lost.Render.Runtime
{

    public class PixelAlignedPass : ScriptableRenderPass
    {
        private PixelAligned setting;
        public PixelAlignedPass(PixelAligned _setting)
        {
            renderPassEvent = RenderPassEvent.BeforeRendering;
            setting = _setting;
        }

        private void SetPixelAligned(CommandBuffer cmd, ref CameraData cameraData)
        {
            var pixelSize = setting.PixelSize;
            var lightLevel = setting.LightLevel;
            var camera = cameraData.camera;
            var isOrtho = camera.orthographic;
            var width = (int)(cameraData.pixelWidth * cameraData.renderScale);
            var height = (int)(cameraData.pixelHeight * cameraData.renderScale);
            Vector4 pixelCS = new Vector4(
                    2.0f / (width / (float)pixelSize),
                    2.0f / (height / (float)pixelSize),
                    0,
                    1
                );
            Vector2 offsetVS = Vector2.zero;
            var renderTargetTexelSize = new Vector4(
                1.0f / width,
                1.0f / height,
                width,
                height
            );
            Matrix4x4 viewMatrix = cameraData.GetViewMatrix();
            if(isOrtho)
            {
                Matrix4x4 inverseViewMatrix = Matrix4x4.Inverse(viewMatrix);
                Matrix4x4 gpuProjectionMatrix = cameraData.GetGPUProjectionMatrix();
                Matrix4x4 inverseProjectionMatrix = Matrix4x4.Inverse(gpuProjectionMatrix);
                Vector4 screenCenterCS = new Vector4(
                    0,
                    0,
                    0,
                    1
                );
                
                Vector4 screenCenterVS = inverseProjectionMatrix * screenCenterCS;
                Vector4 pixelVS = inverseProjectionMatrix * pixelCS;
                Vector2 pixelSizeVS = new Vector2(
                    pixelVS.x / pixelVS.w - screenCenterVS.x / screenCenterVS.w,
                    pixelVS.y / pixelVS.w - screenCenterVS.y / screenCenterVS.w
                );

                Vector4 originWS = new Vector4(0, 0, 0, 1);
                Vector4 originVS = viewMatrix * originWS;
                offsetVS = new Vector2
                (
                    Mathf.FloorToInt(originVS.x / pixelSizeVS.x) * pixelSizeVS.x - originVS.x,
                    Mathf.FloorToInt(originVS.y / pixelSizeVS.y) * pixelSizeVS.y - originVS.y
                );
                viewMatrix[0, 3] += offsetVS[0];
                viewMatrix[1, 3] += offsetVS[1];
            }

            var vp = cameraData.GetGPUProjectionMatrix() * viewMatrix;
            var invVP = vp.inverse;
            cmd.SetGlobalVector(ShaderConstant._PixelSizeCS, pixelCS);
            //cmd.SetGlobalVector(ShaderConstant._CameraOffsetVS, offsetVS);
            cmd.SetGlobalMatrix(ShaderConstant._MatrixV_AlignedPixel, viewMatrix);
            cmd.SetGlobalMatrix(ShaderConstant._MatrixInvV_AlignedPixel, viewMatrix.inverse);
            cmd.SetGlobalMatrix(ShaderConstant._MatrixVP_AlignedPixel, vp);
            cmd.SetGlobalMatrix(ShaderConstant._MatrixInvVP_AlignedPixel, invVP);
            cmd.SetGlobalVector(ShaderConstant._RenderTarget_TexelSize, renderTargetTexelSize);
            cmd.SetGlobalInt(ShaderConstant._PixelSize, pixelSize);
            cmd.SetGlobalInt(ShaderConstant._LightLevel, lightLevel);
            cmd.SetGlobalFloat(ShaderConstant._MinLDotN, setting.MinLDotN);
            cmd.SetGlobalFloat(ShaderConstant._DitherWidth, setting.DitherWidth);
            cmd.SetGlobalFloat(ShaderConstant._RenderScale, cameraData.renderScale);
            cmd.SetGlobalInt(ShaderConstant._ShadowPixelSize, setting.ShadowPixelSize);
            if(pixelSize > 1) cmd.EnableShaderKeyword(ShaderConstant._PIXELATE);
            else cmd.DisableShaderKeyword(ShaderConstant._PIXELATE);

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(setting.name);
            SetPixelAligned(cmd, ref renderingData.cameraData);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
