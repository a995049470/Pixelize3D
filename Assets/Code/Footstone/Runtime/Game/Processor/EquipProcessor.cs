using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class EquipProcessor : SimpleGameEntityProcessor<EquipLabelComponent>
    {
        public EquipProcessor() : base()
        {
            Order = ProcessorOrder.EquipOrPull;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var equipLableComp = kvp.Value.Component1;
                var target = equipLableComp.Target;
                if(target != null && !equipLableComp.IsEffect)
                {
                    equipLableComp.IsEffect = true;   
                    var entity = equipLableComp.Entity;
                    var targetUID = equipLableComp.Target.Id;
                    equipLableComp.CacheComponents = entity.GetEntityComponents(com => 
                    {
                        bool isSuccess = false;
                        if(com is ITakeEffectOnEquipOrPullFrame effect)
                        {
                            isSuccess = true;
                            effect.SetTimePoint(equipLableComp.IsEquipping, targetUID);
                        };
                        return isSuccess;
                    });
                    cmd.MoveEntityComponents(equipLableComp.CacheComponents, target);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
