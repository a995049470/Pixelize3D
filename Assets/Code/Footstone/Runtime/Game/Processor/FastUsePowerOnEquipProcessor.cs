using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FastUsePowerOnEquipProcessor : SimpleGameEntityProcessor<FastUsePowerComponent, PowerReceiverComponent, PlayerControllerComponent>
    {   
        public FastUsePowerOnEquipProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnEquiping;
        }
        
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                if(powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    var itemData = bagData.GetBagGridItemData(bagData.FastSelectIndex);
                    if(itemData.IsVaild())
                    {
                        var uid = itemData.UID;
                        var index = itemData.GridIndex;
                        var controllerComp = kvp.Value.Component3;
                        controllerComp.ItemUID = uid;
                        controllerComp.GridIndex = index;
                        cmd.AddEntityComponent<FastUseComponent>(entity, comp => 
                        {
                            comp.Receicver.ReceivePower(powerComp.PowerUID);
                        });

                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
