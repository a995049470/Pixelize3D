using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class UniqueChestProcessor : SimpleGameEntityProcessor<UniqueChestComponent, ChestComponent, TagComponent, InteractiveLabelComponent, InteractiveComponent>
    {
        private InteractiveRecorderProcessor interactiveRecorderProcessor;

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            interactiveRecorderProcessor = GetProcessor<InteractiveRecorderProcessor>();
        }

        public UniqueChestProcessor() : base()
        {
            Order = ProcessorOrder.UniqueChest;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var chestComp = kvp.Value.Component2;
                var interactiveComp = kvp.Value.Component5;
                if(interactiveComp.IsTriggerEffect)
                {
                    var tagComp = kvp.Value.Component3;
                    //准备开箱
                    if(!chestComp.IsOpen)
                    {
                        var key = $"{tagComp.Key}_{GameConstant.Suffix_Open}";
                        interactiveRecorderProcessor.SingleComponent.UniqueTriggered(key);
                    }
                    //回收箱子让箱子不再刷新
                    else
                    {
                        var key = $"{tagComp.Key}";
                        interactiveRecorderProcessor.SingleComponent.UniqueTriggered(key);
                    }
                }
            }
        }

    }

}
