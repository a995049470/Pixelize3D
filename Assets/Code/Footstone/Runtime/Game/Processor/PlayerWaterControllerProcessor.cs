using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class PlayerWaterControllerProcessor : SimpleGameEntityProcessor<WaterComponent, ActionMaskComponent, PlayerControllerComponent, RotateComponent>
    {
        private InputSettingProcessor inputSettingProcessor;
        public PlayerWaterControllerProcessor() : base()
        {
            Order = ProcessorOrder.WaterCommandInput;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            inputSettingProcessor = GetProcessor<InputSettingProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var actionMaskComp = kvp.Value.Component2;
                var wateringComp = kvp.Value.Component1;
                var ctrlComp = kvp.Value.Component3;
                var rotateComp = kvp.Value.Component4;

                if (!wateringComp.IsWatering && actionMaskComp.IsActionEnable(ActionFlag.Water))
                {
                    var cmd = ctrlComp.CurrentFrameInputCommand;
                    if ((cmd & InputCommand.Action) > 0)
                    {
                        var dir = Vector3.zero;
                        if ((cmd & InputCommand.ActionUp) > 0) dir.z += 1;
                        else if ((cmd & InputCommand.ActionDown) > 0) dir.z -= 1;
                        else if ((cmd & InputCommand.ActionLeft) > 0) dir.x -= 1;
                        else if ((cmd & InputCommand.ActionRight) > 0) dir.x += 1;
                        wateringComp.StartWater(dir);
                        rotateComp.FaceDirection = dir;
                        actionMaskComp.DisabledAction(ActionFlag.Move | ActionFlag.Attack);
                        ctrlComp.ActionSuccess();
                    }
                }
                
            }
        }
    }

}