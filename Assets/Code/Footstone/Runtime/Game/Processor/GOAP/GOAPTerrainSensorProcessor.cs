using Lost.Runtime.Footstone.Core;
namespace Lost.Runtime.Footstone.Game
{
    public class GOAPTerrainSensorProcessor : SimpleGameEntityProcessor<GOAPTerrainSensorComponent, GOAPAgentComponent, VelocityComponent>
    {
        private PlotProcessor plotProcessor;

        public GOAPTerrainSensorProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            plotProcessor = GetProcessor<PlotProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var velocityComp = kvp.Value.Component3;
                var agentComp = kvp.Value.Component2;
                var pos = velocityComp.TargetPos;
                var terrainId = plotProcessor.IsPlot(pos) ? 1 : 0;
                agentComp.WorldStatus.Set(GOAPStatusFlag.StandTerrain, terrainId);
            }
        }
    }
}
