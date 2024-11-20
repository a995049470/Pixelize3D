using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
namespace Lost.Runtime.Footstone.Game
{
    using UniqueChestCheckData = GameData<UniqueChestComponent, ChestComponent, PossibleDroppedComponent, TagComponent, CostComponent>;
    
    public class UniqueChestCheckProcessor : SimpleGameEntityProcessor<UniqueChestComponent, ChestComponent, PossibleDroppedComponent, TagComponent, CostComponent>
    {
        private HashSet<UniqueChestCheckData> newDatas = new();
        private InteractiveRecorderProcessor interactiveRecorderProcessor;

        public UniqueChestCheckProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
        }   

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            interactiveRecorderProcessor = GetProcessor<InteractiveRecorderProcessor>();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if(newDatas.Count > 0)
            {
                foreach (var data in newDatas)
                {
                    var tagComp = data.Component4;
                    var key = $"{tagComp.Key}_{GameConstant.Suffix_Open}";
                    bool isTrigger = interactiveRecorderProcessor.SingleComponent.IsUniqueTriggered(key);
                    if(isTrigger)
                    {
                        var chestComp = data.Component2;
                        var possibleDroppedComp = data.Component3;
                        var costComp = data.Component5;
                        chestComp.IsOpen = true;
                        possibleDroppedComp.IsDropped = true;
                        costComp.IsSuccessCost = true;
                    }
                }
                newDatas.Clear();
            }
        }

        protected override void OnEntityComponentAdding(Entity entity, UniqueChestComponent component, UniqueChestCheckData data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            newDatas.Add(data);
        }

        protected override void OnEntityComponentRemoved(Entity entity, UniqueChestComponent component, UniqueChestCheckData data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            newDatas.Remove(data);
        }
    }

}
