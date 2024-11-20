using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class DigProcessor : SimpleGameEntityProcessor<DigComponent, ActionMaskComponent, RotateComponent>
    {
        //10°的cos值
        private const float cos10 = 0.9848f;

        public DigProcessor() : base()
        {
            Order = ProcessorOrder.Dig;
        }

        //偏差小于10°
        private bool IsCorrectFaceDir(Vector3 faceDir, Vector3 attackDir)
        {
            return Vector3.Dot(faceDir, attackDir) > cos10;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var digComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(digComp.IsDiging)
                {
                    bool isDigUpdate = true;
                    bool isInvokeDig = false;
                    var rotateComp = kvp.Value.Component3;
                    if(!actionMaskComp.IsActionEnable(ActionFlag.Dig) || 
                    rotateComp.FaceDirection != digComp.DigDir)
                    {
                        digComp.ForceCompleteDig();
                        isDigUpdate = false;
                    }

                    if(!IsCorrectFaceDir(digComp.Entity.Transform.Forward, digComp.DigDir))
                    {
                        isDigUpdate = false;
                    }

                    if(isDigUpdate)
                    {
                        digComp.DigUpdate(time.DeltaTime, out isInvokeDig, out var effectKey);
                    }

                    if(isInvokeDig)
                    {
                        var position = digComp.Entity.Transform.Position + digComp.DigDir;
                        var num = physicsSystem.SphereCastNonAlloc(position, 0.2f, castColliders, GameConstant.CropLayer);
                        if(num > 0) 
                        {
                            var collider = castColliders[0];
                            var targetEntityComp = collider.GetComponent<TargetEntity>();
                            var target = targetEntityComp?.Target;
                            if(target)
                            {
                                cmd.AddEntityComponent<DigLabelComponent>(target, comp => 
                                {
                                    comp.Dig(digComp.Entity);
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
