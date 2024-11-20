using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class UniqueInteractiveTriggerProcessor : SimpleGameEntityProcessor<InteractiveComponent, UniqueComponent, TagComponent, InteractiveLabelComponent>
    {
        private InteractiveRecorderProcessor recorderProcessor;

        public UniqueInteractiveTriggerProcessor() : base()
        {
            Order = ProcessorOrder.UniqueInteractiveTrigger;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            recorderProcessor = GetProcessor<InteractiveRecorderProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var interactionComp = kvp.Value.Component1;
                if(interactionComp.IsTriggerEffect)
                {
                    var tagComp = kvp.Value.Component3;
                    recorderProcessor.SingleComponent.UniqueTriggered(tagComp.Key);
                }
            }
        }

    }

}
