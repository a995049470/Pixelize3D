using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AutoRecycleEquipableProcessor : SimpleGameEntityProcessor<EquipLabelComponent, AutoRecycleComponent>
    {
        public AutoRecycleEquipableProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var equipableComp = kvp.Value.Component1;
                if(!equipableComp.IsEquipping && equipableComp.IsEffect)
                {
                    var recycleComp = kvp.Value.Component2;
                    recycleComp.RecycleEntity(cmd, equipableComp.Entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
