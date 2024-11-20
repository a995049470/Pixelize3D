using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class PlayerBuildPlotControllerProcessor : SimpleGameEntityProcessor<BuildPlotComponent, ActionMaskComponent, PlayerControllerComponent, RotateComponent>
    {
        public PlayerBuildPlotControllerProcessor() : base()
        {
            Order = ProcessorOrder.BuildPlotCommandInput;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var buildPlotComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                var ctrlComp = kvp.Value.Component3;
                
                if (!buildPlotComp.IsBuilding && actionMaskComp.IsActionEnable(ActionFlag.BuildPlot))
                {
                    var cmd = ctrlComp.CurrentFrameInputCommand;
                    if ((cmd & InputCommand.Action) > 0)
                    {
                        var dir = Vector3.zero;
                        if ((cmd & InputCommand.ActionUp) > 0) dir.z += 1;
                        else if ((cmd & InputCommand.ActionDown) > 0) dir.z -= 1;
                        else if ((cmd & InputCommand.ActionLeft) > 0) dir.x -= 1;
                        else if ((cmd & InputCommand.ActionRight) > 0) dir.x += 1;
                        ctrlComp.ActionSuccess();
                        var position = actionMaskComp.Entity.Transform.Position + dir;
                        position = PositionUtil.CorrectPosition(position);

                        var gridIndex = ctrlComp.GridIndex;
                        var itemUID = ctrlComp.ItemUID;
                        var itemData = bagData.GetBagGridItemData(gridIndex);
                        var entityKey = itemData.IsVaild() ? itemData.GetEntityKey() : "";
                        buildPlotComp.StartBuilding(entityKey, gridIndex, itemUID, dir, position);
                        var rotateComp = kvp.Value.Component4;
                        rotateComp.FaceDirection = dir;

                    }
                }
                
            }
        }

    }
}
