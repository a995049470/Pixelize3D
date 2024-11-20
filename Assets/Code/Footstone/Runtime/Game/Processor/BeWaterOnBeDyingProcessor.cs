using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class BeWaterOnBeDyingProcessor : SimpleGameEntityProcessor<BeDyingComponent, WaterLabelComponent>
    {
        public BeWaterOnBeDyingProcessor() : base()
        {
            Order = ProcessorOrder.BeWater;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var beDyingComp = kvp.Value.Component1;
                var waterLabelComp = kvp.Value.Component2;
                if(waterLabelComp.IsWatered)
                {
                    beDyingComp.IsWillDie = true;
                    waterLabelComp.IsWatered = false;
                }
                cmd.RemoveEntityComponent(waterLabelComp);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}