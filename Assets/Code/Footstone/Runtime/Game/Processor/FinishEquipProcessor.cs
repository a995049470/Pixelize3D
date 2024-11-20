using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class FinishEquipProcessor : SimpleGameEntityProcessor<EquipLabelComponent>
    {
        public FinishEquipProcessor() : base()
        {
            Order = ProcessorOrder.FinshEquipOrPull;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var equipLableComp = kvp.Value.Component1;
                //var ownerComp = kvp.Value.Component2;
                if(equipLableComp.IsEffect)
                {
                    //cmd.RemoveEntityComponent(ownerComp);
                    // if(equipableComp.IsEquipping)
                    // {
                    // }
                    // //脱装备时只需要清空Target，等待回收时销毁OwnerComponent
                    // else
                    // {
                    //     ownerComp.Target = null;
                    // }
                    var entity = equipLableComp.Entity;
                    cmd.MoveEntityComponents(equipLableComp.CacheComponents, entity);
                    equipLableComp.CacheComponents = null;
                    
                    if(equipLableComp.IsEquipping)
                    {
                        cmd.RemoveEntityComponent(equipLableComp);
                    }
                }
                
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
