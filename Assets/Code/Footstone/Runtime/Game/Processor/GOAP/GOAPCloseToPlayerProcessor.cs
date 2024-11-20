using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPCloseToPlayerProcessor : SimpleGameEntityProcessor<GOAPCloseToPlayerComponent, GOAPAgentComponent, VelocityComponent, ActionMaskComponent, RotateComponent>
    {
        private PlayerProcessor playerProcessor;
        private AStarProcessor astarPorcessor;
        private VelocityProcessor velocityProcessor;

        public GOAPCloseToPlayerProcessor() : base()
        {
            Order = ProcessorOrder.MoveCommandInput;
        }

        

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            playerProcessor = GetProcessor<PlayerProcessor>();
            astarPorcessor = GetProcessor<AStarProcessor>();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        public override void Update(GameTime time)
        {
            
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var agentComp = kvp.Value.Component2;
                bool isFinsish = false;
                bool isSuccess = false;
                var actioComp = kvp.Value.Component1;
                {
                    var playerPosition = playerProcessor.Target.Position;
                    var currentPosition = kvp.Key.Entity.Transform.Position;
                    var distance = Vector3.Distance(playerPosition, currentPosition);
                    var maskComp = kvp.Value.Component4;
                    var velocityComp = kvp.Value.Component3;
                    
                    if(distance < kvp.Value.Component1.TargetDistance)
                    {
                        isSuccess = true;
                        isFinsish = true;
                    }
                    else if(MoveUtil.IsMoveable(maskComp, velocityComp))
                    {
                        var startPosition = velocityComp.MoveStartPos;
                        var endPosition = playerProcessor.Target.Position;

                        var isFind = aStarSystem.TryFindNextPoistion(startPosition, endPosition, 1, out var nextPosition);
                        if(isFind)
                        {
                            var rotateComp = kvp.Value.Component5;
                            var dir = MoveUtil.CaluteMoveDir(startPosition, nextPosition);
                            MoveUtil.MoveStart(velocityComp, rotateComp, maskComp, physicsSystem, velocityProcessor, dir);
                        }
                    }
                }

                if(isFinsish)
                {
                    //cmd.RemoveEntityComponent(actioComp);
                    agentComp.Task.FinshCurrentAction(isSuccess);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
