using System;
using Lost.Runtime.Footstone.Core;
using UnityEngine;
using Random = System.Random;

namespace Lost.Runtime.Footstone.Game
{
    
    public class GOAPStrollProcessor : SimpleGameEntityProcessor<GOAPStrollComponent, GOAPAgentComponent, VelocityComponent, ActionMaskComponent, RotateComponent>
    {
        private Vector3[] directions = new Vector3[]
            {
                 Vector3.right,
                -Vector3.right,
                 Vector3.forward,
                -Vector3.forward
            };
        //反向   转向   方向不变的权重
        int[] weights = new int[] { 1, 50, 1000 };
        public GOAPStrollProcessor() : base()
        {
            Order = ProcessorOrder.MoveCommandInput;
        }
        
        private AStarProcessor aStarProcessor;
        private VelocityProcessor velocityProcessor;

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            aStarProcessor = GetProcessor<AStarProcessor>();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var agentComp = kvp.Value.Component2;
                var worldStatus = agentComp.WorldStatus;
                bool isFinsh = false;
                var velocityComp = kvp.Value.Component3;
                var actionMaskComp = kvp.Value.Component4;
                var rotateComp = kvp.Value.Component5;
                
                if(MoveUtil.IsMoveable(actionMaskComp, velocityComp))
                {
                    var origin = velocityComp.TargetPos;
                    
                    bool isSuccess = MoveUtil.TryRandomGetNextMoveDir(
                        origin, rotateComp.FaceDirection, directions, 
                        weights, physicsSystem, velocityProcessor, random, out var nextMoveDir
                    );
                    if(isSuccess)
                        MoveUtil.MoveStart(velocityComp, rotateComp, actionMaskComp, physicsSystem, velocityProcessor, nextMoveDir);
                }
                
                if(isFinsh)
                {
                    var strollComp = kvp.Value.Component1;
                    cmd.RemoveEntityComponent(strollComp);
                    //agentComp.Task.FinshCurrentAction(true);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
    }
