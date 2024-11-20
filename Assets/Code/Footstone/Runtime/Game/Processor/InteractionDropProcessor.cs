using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    

    //交互直接获取掉落物
    public class InteractionDropProcessor : SimpleGameEntityProcessor<InteractiveComponent, PossibleDroppedComponent, InteractiveLabelComponent>
    {
        public InteractionDropProcessor() : base()
        {
            Order = ProcessorOrder.Drop;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                
                var interactiveComp = kvp.Value.Component1;
                var possibleDroppedComp = kvp.Value.Component2;
                var ownerComp = kvp.Value.Component3;
                var target = ownerComp.Target;
                if(interactiveComp.IsTriggerEffect && !possibleDroppedComp.IsDropped && target)
                {
                    possibleDroppedComp.IsDropped = true;
                    possibleDroppedComp.ReceiveDroppedItems(cmd, bagData, target, random);
                    // possibleDroppedComp.CreateDroppedItemList(random);
                    // var droppedItems = possibleDroppedComp.DroppedItems;
                    // foreach (var droppedItem in droppedItems)
                    // {
                    //     switch (droppedItem.Flag)
                    //     {
                    //         case DroppedItemFlag.EnterBag : 
                    //             bagData.ReceiveItem(droppedItem.Name, droppedItem.Count);
                    //             break;
                    //         case DroppedItemFlag.Effective :
                    //             cmd.InstantiateEntity(droppedItem.Name, ResFlag.Entity_Power, Vector3.zero, 0, entity =>
                    //             {
                    //                 entity.GetOrCreate<BufferLabelComponent>().Target = target;
                    //             });
                    //             break;
                    //     }
                    // }
                    
                    // var position = possibleDroppedComp.Entity.Transform.Position;
                    // var uid = uniqueIdManager.CreateUniqueId();
                    // cmd.InstantiateEntity(droppedItemKey, ResFlag.Entity_Drop, position, 0, entity =>
                    // {
                    //     var droppedItemComp = entity.GetOrCreate<DroppedItemCopmponent>();
                    //     droppedItemComp.AddDroppedItem(possibleDroppedComp.DroppedItems);
                    // });
                    
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
