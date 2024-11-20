using System.Diagnostics;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class BuildPlotProcessor : SimpleGameEntityProcessor<BuildPlotComponent, ActionMaskComponent, RotateComponent>
    {
        private PlotProcessor plotProcessor;
        private TerrainTileProcessor terrainTileProcessor;
        private PlotConfigProcessor plotConfigProcessor;

        public BuildPlotProcessor() : base()
        {
            Order = ProcessorOrder.BuildPlot;
        }
        

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            plotProcessor = GetProcessor<PlotProcessor>();
            terrainTileProcessor = GetProcessor<TerrainTileProcessor>();
            plotConfigProcessor = GetProcessor<PlotConfigProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var buildPlotComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                var rotateComp = kvp.Value.Component3;
                if(buildPlotComp.IsBuilding)
                {
                    if(!actionMaskComp.IsActionEnable(ActionFlag.BuildPlot) || 
                        buildPlotComp.BuildDir != rotateComp.FaceDirection)
                    {
                        buildPlotComp.IsBuilding = false;
                    }
                }
                if(buildPlotComp.IsBuilding && 
                DirectionUtil.IsCorrectFaceDir(buildPlotComp.Entity.Transform.Forward, buildPlotComp.BuildDir))
                {
                    buildPlotComp.IsBuilding = false;
                    var gridIndex = buildPlotComp.BagGridIndex;
                    var itemUID = buildPlotComp.ItemUID;
                    var position = buildPlotComp.BuildPosition;
                    if(!plotProcessor.IsPlot(position))
                    {
                        if(bagData.TryLoseItem(gridIndex, itemUID, 1))
                        {
                            terrainTileProcessor.OnBuildPlot(position);
                            var plotKey = plotConfigProcessor.SingleComponent.EntityKey_Plot;
                            cmd.InstantiateEntity(plotKey, ResFlag.Entity_Env, position, 0);
                        }
                    }
                }
            }

            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
