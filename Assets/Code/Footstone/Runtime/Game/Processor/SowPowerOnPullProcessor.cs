using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class SowPowerOnPullProcessor : SimpleGameEntityProcessor<SowPowerComponent, PowerReceiverComponent, PlayerControllerComponent, SowComponent>
    {
        public SowPowerOnPullProcessor() : base()
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
                var sowComp = kvp.Value.Component4;
                if (!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    controllerComp.ItemUID = 0;
                    controllerComp.GridIndex = 0;
                    if(sowComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        sowComp.ForceCompleteSow();
                        cmd.RemoveEntityComponent(sowComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
