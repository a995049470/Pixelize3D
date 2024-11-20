using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AutoRecyclePlantProcessor : SimpleGameEntityProcessor<LandPlantComponent, AutoRecycleComponent>
    {
        public AutoRecyclePlantProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var plantComp = kvp.Value.Component1;
                var recycleComp = kvp.Value.Component2;
                if(plantComp.CurrentPlantFlag == PlantFlag.Death)
                {
                    plantComp.CurrentPlantFlag = PlantFlag.Seed;
                    plantComp.RemainGrowthPower = 0;
                    plantComp.AdultStateSpeedUpProgress = 0;
                    recycleComp.RecycleEntity(cmd, recycleComp.Entity);
                }
           }
           cmd.Execute();
           commandBufferManager.Release(cmd);
        }
    }
}
