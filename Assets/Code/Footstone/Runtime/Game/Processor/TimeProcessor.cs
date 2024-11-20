using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class TimeProcessor : SingleComponentProcessor<TimeComponent>
    {
        private LandPlantProcessor croplandProcessor;
        public TimeProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            croplandProcessor = GetProcessor<LandPlantProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var timeComp = kvp.Value.Component1;
                var sceneName = gameSceneManager.CurrentScene;
                timeComp.SceneCostPowerDic.TryGetValue(sceneName, out var costPower);
                var remainPower = timeComp.TotalGrowthPower - costPower;
                if(remainPower > 0)
                {
                    croplandProcessor.ReceiveGrowthPower(remainPower);
                    costPower += remainPower;
                    timeComp.SceneCostPowerDic[sceneName] = costPower;
                }
            }
        }
        
        
    }
}
