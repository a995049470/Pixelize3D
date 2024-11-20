using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 处理传送
    /// </summary>
    public class TransportProcessor : SimpleGameEntityProcessor<TransportComponent, VelocityComponent>
    {
        private VelocityProcessor velocityProcessor;
        private CameraFollowPlayerProcessor cameraFollowPlayerProcessor;

        public TransportProcessor() : base()
        {
            Order = ProcessorOrder.Transport;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            velocityProcessor = GetProcessor<VelocityProcessor>();
            cameraFollowPlayerProcessor = GetProcessor<CameraFollowPlayerProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var transportComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component2;
                var transComp = kvp.Key.Entity.Transform;
                var src = transComp.Position;
                var dst = transportComp.TargetPosition;
                if(velocityProcessor.TryTransport(velocityComp, dst))
                {
                    var entity = transComp.Entity;
                    cameraFollowPlayerProcessor.OnEntityTransport(entity);
                    transComp.Position = transportComp.TargetPosition;
                    velocityComp.SetVaildTargetPos(transportComp.TargetPosition);
                }
                cmd.RemoveEntityComponent(transportComp);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
