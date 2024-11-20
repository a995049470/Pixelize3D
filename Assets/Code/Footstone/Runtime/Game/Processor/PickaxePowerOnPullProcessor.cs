using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PickaxePowerOnPullProcessor : SimpleGameEntityProcessor<PickaxePowerComponent, PowerReceiverComponent, PickaxeComponent>
    {
        public PickaxePowerOnPullProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnPulling;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var pickaxeComp = kvp.Value.Component3;
                if (!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    if(pickaxeComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        pickaxeComp.ForceCompletePickaxe();
                        cmd.RemoveEntityComponent(pickaxeComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
