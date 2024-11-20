using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    

    /// <summary>
    /// 死亡掉落物体组件
    /// </summary>
    public class DeadDropProcessor : SimpleGameEntityProcessor<DeadComponent, PossibleDroppedComponent, HitPointComponent>
    {
        private string droppedItemKey { get => GameConstant.EntityKey_Dropped; }
        public DeadDropProcessor() : base()
        {
            Order = ProcessorOrder.Drop;
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var hpComp = kvp.Value.Component3;
                var deadComp = kvp.Value.Component1;
                var possibleDroppedComp = kvp.Value.Component2;
                if(hpComp.IsDead && !possibleDroppedComp.IsDropped)
                {
                    possibleDroppedComp.CreateDroppedItemList(random);
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
