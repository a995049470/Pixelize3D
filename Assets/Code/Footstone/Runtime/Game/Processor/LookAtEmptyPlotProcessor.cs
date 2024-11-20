

using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class LookAtEmptyPlotProcessor : SimpleGameEntityProcessor<LookAtEmptyPlotComponent, RotateComponent, VelocityComponent>
    {
        private AStarProcessor aStarProcessor;
        private PlotProcessor plotProcessor;
        private static Vector3[] directions = new Vector3[]
        {
            new Vector3(1, 0, 0), new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1), new Vector3(0, 0, -1),
        };
        public LookAtEmptyPlotProcessor() : base()
        {
            Order = ProcessorOrder.FrameEnd;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            aStarProcessor = GetProcessor<AStarProcessor>();
            plotProcessor = GetProcessor<PlotProcessor>();

        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var lookAtEmptyPlotComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component3;
                var rotateComp = kvp.Value.Component2;
                var starPos = velocityComp.TargetPos;
                for (int i = 0; i < directions.Length; i++)
                {
                    var dir = directions[i];
                    var pos = PositionUtil.CorrectPosition(starPos + dir);
                    bool isEmptyPlot = aStarSystem.IsWayPoint(pos) && plotProcessor.IsPlot(pos);
                    if(isEmptyPlot)
                    {
                        rotateComp.FaceDirection = dir;
                        break;
                    }
                }
                cmd.RemoveEntityComponent(lookAtEmptyPlotComp);
            }
            cmd.Execute();
        }
    }
}
