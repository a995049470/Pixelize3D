using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class BuildPlotPowerOnPullProcessor :  SimpleGameEntityProcessor<BuildPlotPowerComponent, PowerReceiverComponent, BuildPlotComponent, PlayerControllerComponent>
    {
        public BuildPlotPowerOnPullProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnPulling;
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                var buildPlotComp = kvp.Value.Component3;
                if (!powerComp.IsEquipping)
                {
                    var controllerComp = kvp.Value.Component4;
                    controllerComp.ItemUID = 0;
                    controllerComp.GridIndex = 0;

                    var entity = kvp.Key.Entity;
                    if(buildPlotComp.Receicver.LostPower(powerComp.PowerUID))
                    {
                        buildPlotComp.ForceCompleteBuild();
                        cmd.RemoveEntityComponent(buildPlotComp);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
