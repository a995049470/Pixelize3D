using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FoodPowerOnPullProcessor : SimpleGameEntityProcessor<FoodPowerComponent, PowerReceiverComponent, PlayerControllerComponent, EatComponent>
    {
        public FoodPowerOnPullProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnPulling;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var controllerComp = kvp.Value.Component3;
                var eatComp = kvp.Value.Component4;
                if (!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    controllerComp.ItemUID = 0;
                    controllerComp.GridIndex = 0;
                    if(eatComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        eatComp.ForceCompleteEat();
                        cmd.RemoveEntityComponent(eatComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
