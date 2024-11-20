using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class AttackProcessor : SimpleGameEntityProcessor<AttackComponent, RotateComponent, EnergyComponent, ActionMaskComponent>
    {
        //10°的cos值
        private const float cos10 = 0.9848f;

        public AttackProcessor() : base()
        {
            Order = ProcessorOrder.Attack;
        }

    
        //偏差小于10°
        private bool IsCorrectFaceDir(Vector3 faceDir, Vector3 attackDir)
        {
            return Vector3.Dot(faceDir, attackDir) > cos10;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var attackComp = kvp.Value.Component1;
                var rotateComp = kvp.Value.Component2;
                var transComp = kvp.Key.Entity.Transform;
                var maskComp = kvp.Value.Component4;
    
                if(attackComp.IsAttacking)
                {
                    bool isContinueAttack = true;
                    attackComp.IsPlayAttack = true;
                    if(!maskComp.IsActionEnable(ActionFlag.Attack) || 
                       rotateComp.FaceDirection != attackComp.AttackDir)
                    {
                        var energyComp = kvp.Value.Component3;
                        energyComp.ClearFatigueTime();
                        isContinueAttack = false;
                        attackComp.ForceCompleteAttack();
                    
                    }
                    if(isContinueAttack && !attackComp.IsCorrectFaceDir)
                    {
                        attackComp.IsCorrectFaceDir = IsCorrectFaceDir(transComp.Forward, attackComp.AttackDir);
                        isContinueAttack &= attackComp.IsCorrectFaceDir;
                    }


                    if (isContinueAttack && attackComp.TryStartCostEnergy())
                    {
                        attackComp.EndCostEnery();
                        var energyComp = kvp.Value.Component3;
                        if (!energyComp.TryCostEnerge(attackComp.Data.AttackCostEnergy, 0))
                        {
                            attackComp.ForceCompleteAttack();
                            isContinueAttack = false;
                        }
                    }

                    attackComp.IsPlayAttack = isContinueAttack;
                    if(isContinueAttack)
                    {
                        var speed = attackComp.AttackSpeed.GetFinalValue(time.FrameCount);
                        attackComp.TryCompleteAttack(time.DeltaTime * speed);
                    }
                }
                else 
                    attackComp.IsPlayAttack = false;
            }
        }
    }
}
