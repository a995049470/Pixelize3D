using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class BeDyingDropProcessor : SimpleGameEntityProcessor<BeDyingComponent, PossibleDroppedComponent>
    {
        private string droppedItemKey = "CustomDroppedItem";
        public BeDyingDropProcessor() : base()
        {
            Order = ProcessorOrder.Drop;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var beDyingComp = kvp.Value.Component1;
                var possibleDroppedComp = kvp.Value.Component2;
                if(beDyingComp.IsWillDie && !possibleDroppedComp.IsDropped)
                {
                   possibleDroppedComp.IsDropped = true;
                   possibleDroppedComp.IsDropped = true;
                    var position = possibleDroppedComp.Entity.Transform.Position;
                    var uid = uniqueIdManager.CreateUniqueId();
                    var droppedItems = possibleDroppedComp.DroppedItems;
                    if(droppedItems.Count > 0)
                    {
                        cmd.InstantiateEntity(droppedItemKey, ResFlag.Entity_Drop, position, 0, entity =>
                        {
                            var droppedItemComp = entity.GetOrCreate<DroppedItemCopmponent>();
                            droppedItemComp.AddDroppedItem(droppedItems);
                        });
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
