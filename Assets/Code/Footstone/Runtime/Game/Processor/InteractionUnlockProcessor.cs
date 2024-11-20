using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //交互解锁
    public class InteractionUnlockProcessor : SimpleGameEntityProcessor<InteractiveComponent, UnlockComponent, InteractiveLabelComponent>
    {
        private LockProcessor lockProcessor;
        public InteractionUnlockProcessor() : base()
        {
            Order = ProcessorOrder.Unlock;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            lockProcessor = GetProcessor<LockProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactionComp = kvp.Value.Component1;
                var unlockComp = kvp.Value.Component2;
                if(interactionComp.IsTriggerEffect)
                {
                    lockProcessor.Unlock(unlockComp.KeyName);   
                }
            }
        }
    }

}
