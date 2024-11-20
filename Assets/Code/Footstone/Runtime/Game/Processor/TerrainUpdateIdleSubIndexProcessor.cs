using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class TerrainUpdateIdleSubIndexProcessor : SimpleGameEntityProcessor<TerrainDetectionComponent, VelocityComponent>
    {
        private PlotProcessor plotProcessor;

        public TerrainUpdateIdleSubIndexProcessor() : base()
        {
            Order = ProcessorOrder.TerrainUpdateIdleSubIndex;
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
                var terrainComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component2;
                var pos = velocityComp.TargetPos;
                velocityComp.IdleSubIndex = plotProcessor.IsPlot(pos) ? terrainComp.IdleSubIndex_Plot : terrainComp.IdleSubIndex_Water;
            }
        }
    }
}
