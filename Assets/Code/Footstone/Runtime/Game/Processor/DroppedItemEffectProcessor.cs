using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class DroppedItemEffectProcessor : SimpleGameEntityProcessor<DroppedItemCopmponent, PickLabelComponent>
    {
        public DroppedItemEffectProcessor() : base()
        {
            Order = ProcessorOrder.DroppedItemEffect;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var droppeItemComp = kvp.Value.Component1;
                var target = kvp.Value.Component2.Target;
                if(target != null)
                {
                    foreach (var droppedItem in droppeItemComp.DroppedItems)
                    {
                        switch (droppedItem.Flag)
                        {
                            case DroppedItemFlag.EnterBag : 
                                bagData.ReceiveItem(droppedItem.Name, droppedItem.Count);
                                break;
                            case DroppedItemFlag.Effective :
                                cmd.InstantiateEntity(droppedItem.Name, ResFlag.Entity_Power, Vector3.zero, 0, entity =>
                                {
                                    entity.GetOrCreate<BufferLabelComponent>().Target = target;
                                });
                                break;
                        }
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
