using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //检查交互物是否加锁
    public class InteractionLockedCheckProcessor : SimpleGameEntityProcessor<InteractiveComponent, LockComponent, InteractiveLabelComponent>
    {
        public InteractionLockedCheckProcessor() : base()
        {
            Order = ProcessorOrder.InteractiveLockedCheck;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactionComp = kvp.Value.Component1;
                var lockComp = kvp.Value.Component2;
                if(lockComp.IsLocked)
                {
                    UnityEngine.Debug.Log($"无法通过");
                }
                interactionComp.TriggerFlag = TriggerFlag.WaitTrigger;
            }
        }
    }

}
