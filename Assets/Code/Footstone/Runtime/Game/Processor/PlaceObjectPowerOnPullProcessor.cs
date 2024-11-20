using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlaceObjectPowerOnPullProcessor : SimpleGameEntityProcessor<PlaceObjectPowerComponent, PowerReceiverComponent, PlayerControllerComponent, PlaceObjectComponent>
    {
        public PlaceObjectPowerOnPullProcessor() : base()
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
                var placeObjectPower = kvp.Value.Component4;
                if (!powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    controllerComp.ItemUID = 0;
                    controllerComp.GridIndex = 0;
                    if(placeObjectPower.Receicver.LostPower(powerComp.PowerUID))
                    {
                        placeObjectPower.ForceCompleteBuild();
                        cmd.RemoveEntityComponent(placeObjectPower);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
