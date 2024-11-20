using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class PlayerSowControllerProcessor : SimpleGameEntityProcessor<SowComponent, ActionMaskComponent, PlayerControllerComponent, RotateComponent>
    {
        private PlaceProcessor placeProcessor;

        public PlayerSowControllerProcessor() : base()
        {
            Order = ProcessorOrder.SowCommandInput;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            placeProcessor = GetProcessor<PlaceProcessor>();
        }


        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var actionMaskComp = kvp.Value.Component2;
                var ctrlComp = kvp.Value.Component3;
                var sowComp = kvp.Value.Component1;
                if (!sowComp.IsSowing && actionMaskComp.IsActionEnable(ActionFlag.Sow))
                {
                    var cmd = ctrlComp.CurrentFrameInputCommand;
                    if((cmd & InputCommand.Action) > 0)
                    {
                        var dir = Vector3.zero;
                        if ((cmd & InputCommand.ActionUp) > 0) dir.z += 1;
                        else if ((cmd & InputCommand.ActionDown) > 0) dir.z -= 1;
                        else if ((cmd & InputCommand.ActionLeft) > 0) dir.x -= 1;
                        else if ((cmd & InputCommand.ActionRight) > 0) dir.x += 1;
                        
                        ctrlComp.ActionSuccess();
                        var position = actionMaskComp.Entity.Transform.Position + dir;
                        var castNum = physicsSystem.SphereCastNonAlloc(position, 0.3f, castColliders, GameConstant.CropLayer);
                        if (castNum > 0)
                        {
                            var collider = castColliders[0];
                            var targetEntityComp = collider.GetComponent<TargetEntity>();
                            var targetEntity = targetEntityComp?.Target;
                            bool isDig = targetEntity?.Get<CroplandComponent>()?.IsDig ?? false;
                            bool isEmpty = placeProcessor.IsEmpty(position);
                            if (isDig && isEmpty)
                            {
                                var gridIndex = ctrlComp.GridIndex;
                                var itemUID = ctrlComp.ItemUID;
                                var itemData = bagData.GetBagGridItemData(gridIndex);
                                var plantKey = itemData.IsVaild() ? itemData.GetEntityKey() : "";
                                if(bagData.IsBagGridItemNumEnough(gridIndex, itemUID, 1))
                                {
                                    sowComp.StartSow(plantKey, targetEntity.Id, dir, gridIndex, itemUID);
                                    var rotateComp = kvp.Value.Component4;
                                    rotateComp.FaceDirection = dir;
                                    actionMaskComp.DisabledAction(ActionFlag.Move | ActionFlag.Attack);
                                }
                            }
                        }
                    }
                }
                
            }
        }
    }
}
