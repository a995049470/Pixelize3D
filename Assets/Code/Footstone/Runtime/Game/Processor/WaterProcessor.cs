using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{


    public class WaterProcessor : SimpleGameEntityProcessor<WaterComponent, ActionMaskComponent, RotateComponent>
    {
        public WaterProcessor() : base()
        {
            Order = ProcessorOrder.Water;
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
                var waterComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(waterComp.IsWatering)
                {
                    bool isWaterUpdate = true;
                    bool isInvokeWater = false;
                    var rotateComp = kvp.Value.Component3;
                    if(!actionMaskComp.IsActionEnable(ActionFlag.Water) ||
                      rotateComp.FaceDirection != waterComp.WaterDir)
                    {
                        waterComp.ForceCompleteWater();
                        isWaterUpdate = false;
                    }

                    if(!IsCorrectFaceDir(waterComp.Entity.Transform.Forward, waterComp.WaterDir))
                    {
                        isWaterUpdate = false;
                    }

                    if(isWaterUpdate)
                    {
                        waterComp.WaterUpdate(time.DeltaTime, out isInvokeWater, out var effectKey);
                    }

                    if(isInvokeWater)
                    {
                        var position = waterComp.Entity.Transform.Position + waterComp.WaterDir;
                        var num = physicsSystem.SphereCastNonAlloc(position, 0.2f, castColliders, GameConstant.CropLayer);
                        if(num > 0) 
                        {
                            var collider = castColliders[0];
                            var targetEntityComp = collider.GetComponent<TargetEntity>();
                            var target = targetEntityComp?.Target;
                            if(target)
                            {
                                cmd.AddEntityComponent<WaterLabelComponent>(target, comp => 
                                {
                                    comp.Water(waterComp.Entity);
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