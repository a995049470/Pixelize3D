using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AnimationClipEventTriggerEndProcessor : SimpleGameEntityProcessor<AnimationClipEventTriggerComponent, AnimationClipEventTriggerLabelComponent>
    {
        public AnimationClipEventTriggerEndProcessor() : base()
        {
            Order = ProcessorOrder.AnimationClipEventTriggerEnd;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var clipEventTriggerComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                if(clipEventTriggerComp.IsTrigger)
                {
                    var entity = ownerComp.Entity;
                    cmd.MoveEntityComponents(clipEventTriggerComp.CacheComponents, entity);
                    cmd.RemoveEntityComponent(ownerComp);
                    clipEventTriggerComp.CacheComponents = null;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
