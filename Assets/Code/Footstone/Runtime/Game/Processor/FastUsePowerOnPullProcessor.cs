using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FastUsePowerOnPullProcessor : SimpleGameEntityProcessor<FastUsePowerComponent, PowerReceiverComponent, PlayerControllerComponent, FastUseComponent>
    {
        public FastUsePowerOnPullProcessor() : base()
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
                var fastUseComp = kvp.Value.Component4;
                if (!powerComp.IsEquipping)
                {
                    controllerComp.ItemUID = 0;
                    controllerComp.GridIndex = 0;
                    if(fastUseComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        fastUseComp.ForceCompleteFastUse();
                        cmd.RemoveEntityComponent(fastUseComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
