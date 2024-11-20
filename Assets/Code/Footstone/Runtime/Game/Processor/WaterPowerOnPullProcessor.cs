using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class WaterPowerOnPullProcessor : SimpleGameEntityProcessor<WaterPowerComponent, PowerReceiverComponent, WaterComponent>
    {
        public WaterPowerOnPullProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnPulling;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var waterComp = kvp.Value.Component3;
                if (!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    if(waterComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        waterComp.ForceCompleteWater();
                        cmd.RemoveEntityComponent(waterComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
