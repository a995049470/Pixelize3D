using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class PickaxeProcessor : SimpleGameEntityProcessor<PickaxeComponent, ActionMaskComponent, RotateComponent>
    {
        public PickaxeProcessor() : base()
        {
            Order = ProcessorOrder.Pickaxe;
        }

        //偏差小于10°
        private bool IsCorrectFaceDir(Vector3 faceDir, Vector3 attackDir)
        {
            return DirectionUtil.IsCorrectFaceDir(faceDir, attackDir);
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var pickaxeComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(pickaxeComp.IsPickaxing)
                {
                    bool isPickaxeUpdate = true;
                    bool isInvokeWater = false;
                    var rotateComp = kvp.Value.Component3;
                    if(!actionMaskComp.IsActionEnable(ActionFlag.Pickaxe) ||
                      rotateComp.FaceDirection != pickaxeComp.PickaxeDir)
                    {
                        pickaxeComp.ForceCompletePickaxe();
                        isPickaxeUpdate = false;
                    }

                    if(!IsCorrectFaceDir(pickaxeComp.Entity.Transform.Forward, pickaxeComp.PickaxeDir))
                    {
                        isPickaxeUpdate = false;
                    }

                    if(isPickaxeUpdate)
                    {
                        pickaxeComp.PickaxeUpdate(time.DeltaTime, out isInvokeWater, out var effectKey);
                    }

                    if(isInvokeWater)
                    {
                        var position = pickaxeComp.Entity.Transform.Position + pickaxeComp.PickaxeDir;
                        var num = physicsSystem.SphereCastNonAlloc(position, 0.2f, castColliders, GameConstant.InteractionLayer);
                        if(num > 0) 
                        {
                            var collider = castColliders[0];
                            var targetEntityComp = collider.GetComponent<TargetEntity>();
                            var target = targetEntityComp?.Target;
                            if(target)
                            {
                                cmd.AddEntityComponent<PickaxeLabelComponent>(target, comp => 
                                {
                                    comp.Pickaxe(pickaxeComp.Entity);
                                });
                                
                            }
                        }
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}