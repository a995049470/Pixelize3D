using Lost.Runtime.Footstone.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{


    public class PlayerMoveControllerProcessor : SimpleGameEntityProcessor<PlayerControllerComponent, VelocityComponent, ActionMaskComponent, RotateComponent>
    {    
        private VelocityProcessor velocityProcessor;
        public PlayerMoveControllerProcessor() : base()
        {
            Order = ProcessorOrder.MoveCommandInput;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            
            foreach(var kvp in ComponentDatas)
            {
            
                var actionMask = kvp.Value.Component3;
                
                var velocity = kvp.Value.Component2;
                var rotate = kvp.Value.Component4;
                var ctrlComp = kvp.Value.Component1;
                var cmd = ctrlComp.CurrentFrameInputCommand;
                var dir = Vector3.zero;
                if(actionMask.IsActionEnable(ActionFlag.Move))
                {
                    if((cmd & InputCommand.Move) > 0)
                    {
                        if((cmd & InputCommand.MoveUp) > 0) dir.z += 1;
                        else if((cmd & InputCommand.MoveDown) > 0) dir.z -= 1;
                        else if((cmd & InputCommand.MoveLeft) > 0) dir.x -= 1;
                        else if((cmd & InputCommand.MoveRight) > 0) dir.x += 1;
                    }
                    MoveUtil.MoveStart(velocity, rotate, actionMask, physicsSystem, velocityProcessor, dir);
                }

            };

            
        }

       


    }
}
