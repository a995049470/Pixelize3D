using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //击退
    public class RepulsedProcessor : SimpleGameEntityProcessor<RepulsedComponent, VelocityComponent, ActionMaskComponent>
    {
        private VelocityProcessor velocityProcessor;
        public RepulsedProcessor() : base()
        {
            Order = ProcessorOrder.Move; 
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var repulsedComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component2;
                bool isContinue = repulsedComp.TryRepulsed(time, velocityComp, velocityProcessor, physicsSystem);
                if(!isContinue)
                {
                    cmd.RemoveEntityComponent(repulsedComp);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
