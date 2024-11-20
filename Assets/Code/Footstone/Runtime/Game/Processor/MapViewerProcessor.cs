using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class MapViewerProcessor : SimpleGameEntityProcessor<MapViewerComponent>
    {
        private MapProcessor mapProcessor;
        private int size = 3;
        private int count { get => size * size; }
        private Vector3[] positions;
        public MapViewerProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
            positions = new Vector3[count];
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            mapProcessor = GetProcessor<MapProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var mapViewerComp = kvp.Value.Component1;
                var pos = PositionUtil.CorrectPosition(mapViewerComp.Entity.Transform.Position);
                if(pos != mapViewerComp.LastPosition)
                {
                    for (int i = 0; i < count; i++)
                    {
                        positions[i] = pos + new Vector3(
                            i % size - size / 2,
                            0,
                            i / size - size / 2
                        );
                    }
                    mapViewerComp.LastPosition = pos;
                    var scene = gameSceneManager.CurrentScene;
                    mapProcessor.SingleComponent?.Data?.AddView(scene, positions);
                }
                
            }
        }
    }
}
