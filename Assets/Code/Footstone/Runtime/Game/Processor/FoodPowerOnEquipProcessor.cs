using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FoodPowerOnEquipProcessor : SimpleGameEntityProcessor<FoodPowerComponent, PowerReceiverComponent, PlayerControllerComponent>
    {
        public FoodPowerOnEquipProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnEquiping;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                if(powerComp.IsEquipping)
                {
                    var entity = powerComp.Entity;
                    var itemData = bagData.GetBagGridItemData(bagData.FastSelectIndex);
                    if(itemData.IsVaild())
                    {
                        var uid = itemData.UID;
                        var index = itemData.GridIndex;
                        var controllerComp = kvp.Value.Component3;
                        controllerComp.ItemUID = uid;
                        controllerComp.GridIndex = index;
                        cmd.AddEntityComponent<EatComponent>(entity, comp => 
                        {
                            comp.Receicver.ReceivePower(powerComp.PowerUID);
                            comp.Duration = powerComp.EatDuration;
                            comp.TotalRecover = powerComp.TotalRecover;
                        });
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);

        }
    }

}
