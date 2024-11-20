using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{


    public class GOAPFollowPlayerProcessor : SimpleGameEntityProcessor<GOAPFollowPlayerComponent, GOAPAgentComponent, VelocityComponent, ActionMaskComponent, RotateComponent>
    {
        private PlayerProcessor playerProcessor;
        private AStarProcessor aStarProcessor;
        private VelocityProcessor velocityProcessor;

        public GOAPFollowPlayerProcessor()
        {
            Order = ProcessorOrder.AICommand;
        }

 

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            playerProcessor = GetProcessor<PlayerProcessor>();
            aStarProcessor = GetProcessor<AStarProcessor>();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        public override void Update(GameTime time)
        {
           
            foreach (var kvp in ComponentDatas)
            {
                var velocityComp = kvp.Value.Component3;
                var actionMaskComp = kvp.Value.Component4;
                var rotateComp = kvp.Value.Component5;
                if(actionMaskComp.IsActionEnable(ActionFlag.Move) && velocityComp.InvaildTargetPos)
                {
                    var startPosition = velocityComp.MoveStartPos;
                    var endPosition = playerProcessor.Target.Position;
                    var isSuccess = aStarSystem.TryFindNextPoistion(startPosition, endPosition, 1, out var nextPosition);
                    if(isSuccess)
                    {
                        var dir = (nextPosition - startPosition).normalized;
                        MoveUtil.MoveStart(velocityComp, rotateComp, actionMaskComp, physicsSystem, velocityProcessor, dir);
                    }
                }
            }
        }

        
    }
}
