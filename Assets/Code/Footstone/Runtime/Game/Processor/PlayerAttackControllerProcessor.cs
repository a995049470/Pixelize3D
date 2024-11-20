using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class PlayerAttackControllerProcessor : SimpleGameEntityProcessor<PlayerControllerComponent, AttackComponent, ActionMaskComponent, RotateComponent>
    {
        private InputSettingProcessor inputSettingProcessor;
        
        public PlayerAttackControllerProcessor() : base()
        {
            Order = ProcessorOrder.AttackCommandInput;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            inputSettingProcessor = GetProcessor<InputSettingProcessor>();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            var dir = Vector3.zero;
            var inputKey = KeyCode.None;
            foreach (var kvp in ComponentDatas)
            {
                var actionMask = kvp.Value.Component3;
                var attack = kvp.Value.Component2;
                var rotate = kvp.Value.Component4;
                var controller = kvp.Value.Component1;
                var key = inputKey;
                
                if (!attack.IsAttacking && actionMask.IsActionEnable(ActionFlag.Attack))
                {
                    var cmd = controller.CurrentFrameInputCommand;
                    if((cmd & InputCommand.Action) > 0)
                    {
                        dir = Vector3.zero;
                        if((cmd & InputCommand.ActionUp) > 0) dir.z += 1;
                        else if((cmd & InputCommand.ActionDown) > 0) dir.z -= 1;
                        else if((cmd & InputCommand.ActionLeft) > 0) dir.x -= 1;
                        else if((cmd & InputCommand.ActionRight) > 0) dir.x += 1;
                        attack.ApplyAttackCommand(dir);
                        rotate.FaceDirection = dir;
                        actionMask.DisabledAction(ActionFlag.Move);
                        controller.ActionSuccess();
                        controller.DisableMoveUntilNoMoveButtonDown = true;
                    }
                    else
                        controller.DisableMoveUntilNoMoveButtonDown = false;
                }
                
            }
            

        }

        
    }
}
