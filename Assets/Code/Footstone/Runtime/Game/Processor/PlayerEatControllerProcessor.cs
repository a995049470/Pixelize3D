using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class PlayerEatControllerProcessor : SimpleGameEntityProcessor<EatComponent, ActionMaskComponent, PlayerControllerComponent, RotateComponent>
    {
        public PlayerEatControllerProcessor() : base()
        {
            Order = ProcessorOrder.EatCommandInput;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var actionMaskComp = kvp.Value.Component2;
                var ctrlComp = kvp.Value.Component3;
                var eatComp = kvp.Value.Component1;
                if (!eatComp.IsEatting && actionMaskComp.IsActionEnable(ActionFlag.Sow))
                {
                    var cmd = ctrlComp.CurrentFrameInputCommand;
                    if((cmd & InputCommand.Action) > 0)
                    {
                        var dir = Vector3.zero;
                        if ((cmd & InputCommand.ActionUp) > 0) dir.z += 1;
                        else if ((cmd & InputCommand.ActionDown) > 0) dir.z -= 1;
                        else if ((cmd & InputCommand.ActionLeft) > 0) dir.x -= 1;
                        else if ((cmd & InputCommand.ActionRight) > 0) dir.x += 1;
                        var rotateComp = kvp.Value.Component4;
                        rotateComp.FaceDirection = dir;
                        var isSuccessUse = bagData.TryLoseItem(ctrlComp.GridIndex, ctrlComp.ItemUID, 1);
                        if(isSuccessUse)
                        {
                            eatComp.StartEatting(dir);
                        }
                    }
                }
                //吞掉输入防止攻击生效
                ctrlComp.ActionSuccess();
            }
        }
    }
}
