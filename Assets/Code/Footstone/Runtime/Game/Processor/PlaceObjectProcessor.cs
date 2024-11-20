using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PlaceObjectProcessor : SimpleGameEntityProcessor<PlaceObjectComponent, ActionMaskComponent, RotateComponent>
    {
        private PlotProcessor plotProcessor;
        private PlaceProcessor placeProcessor;
        private TerrainTileProcessor terrainTileProcessor;
        public PlaceObjectProcessor() : base()
        {
            Order = ProcessorOrder.PlaceObject;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            plotProcessor = GetProcessor<PlotProcessor>();
            placeProcessor = GetProcessor<PlaceProcessor>();
            terrainTileProcessor = GetProcessor<TerrainTileProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var placeObjectComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                var rotateComp = kvp.Value.Component3;
                if(placeObjectComp.IsBuilding)
                {
                    if(!actionMaskComp.IsActionEnable(ActionFlag.PlaceObject) || 
                        placeObjectComp.BuildDir != rotateComp.FaceDirection)
                    {
                        placeObjectComp.IsBuilding = false;
                    }
                }
                if(placeObjectComp.IsBuilding && 
                DirectionUtil.IsCorrectFaceDir(placeObjectComp.Entity.Transform.Forward, placeObjectComp.BuildDir))
                {
                    placeObjectComp.IsBuilding = false;
                    var gridIndex = placeObjectComp.BagGridIndex;
                    var itemUID = placeObjectComp.ItemUID;
                    var position = placeObjectComp.BuildPosition;
                    var scene = gameSceneManager.CurrentScene;
                    bool isVaild = plotProcessor.IsPlaceable(position) && placeProcessor.IsEmpty(position);
                    if(isVaild)
                    {
                        if(bagData.TryLoseItem(gridIndex, itemUID, 1))
                        {
                            terrainTileProcessor.OnPlaceObject(position);
                            if(!string.IsNullOrEmpty(placeObjectComp.ObjectKey))
                            {
                                cmd.InstantiateEntity(placeObjectComp.ObjectKey, ResFlag.Entity_Place, position, 0);
                            }
                        }
                    }
                }
            }

            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
