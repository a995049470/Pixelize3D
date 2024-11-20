using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DigPowerOnPullProcessor : SimpleGameEntityProcessor<DigPowerComponent, PowerReceiverComponent, DigComponent>
    {
        public DigPowerOnPullProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnPulling;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var digComp = kvp.Value.Component3;
                if (!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    if(digComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        digComp.ForceCompleteDig();
                        cmd.RemoveEntityComponent(digComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
