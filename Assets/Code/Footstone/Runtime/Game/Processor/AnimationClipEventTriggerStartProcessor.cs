using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    
    public class AnimationClipEventTriggerStartProcessor : SimpleGameEntityProcessor<AnimationClipEventTriggerComponent, AnimationClipEventTriggerLabelComponent>
    {
        public AnimationClipEventTriggerStartProcessor() : base()
        {
            Order = ProcessorOrder.AnimationClipEventTriggerStart;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var clipEventTriggerComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                if(ownerComp.Target != null)
                {
                    clipEventTriggerComp.IsTrigger = true;
                    var entity = ownerComp.Entity;
                    clipEventTriggerComp.CacheComponents = entity.GetEntityComponents<IAnimationClipEvent>();
                    cmd.MoveEntityComponents(clipEventTriggerComp.CacheComponents, ownerComp.Target);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
