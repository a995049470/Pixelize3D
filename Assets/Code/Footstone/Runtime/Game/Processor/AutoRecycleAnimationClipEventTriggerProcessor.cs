using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AutoRecycleAnimationClipEventTriggerProcessor : SimpleGameEntityProcessor<AnimationClipEventTriggerComponent, AutoRecycleComponent>
    {
        public AutoRecycleAnimationClipEventTriggerProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var triggerComp = kvp.Value.Component1;
                var autoRecycleComp = kvp.Value.Component2;
                if(triggerComp.IsTrigger)
                {
                    var entity = autoRecycleComp.Entity;
                    autoRecycleComp.RecycleEntity(cmd, entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
