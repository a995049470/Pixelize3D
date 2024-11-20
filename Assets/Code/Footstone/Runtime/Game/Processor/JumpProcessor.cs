using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class JumpProcessor : SimpleGameEntityProcessor<VelocityComponent, ActionMaskComponent, EnergyComponent, JumperComponent>
    {
        public JumpProcessor() : base()
        {
            Order = ProcessorOrder.Move; 
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var data = kvp.Value;
                var velocityComp = data.Component1;
                var actionMaskComp = data.Component2;
                var energyComp = kvp.Value.Component3;
                var jumperComp = kvp.Value.Component4;
                var transComp = kvp.Key.Entity.Transform;
                
                bool isMoveable = actionMaskComp.IsActionEnable(ActionFlag.Move);       
                isMoveable &= velocityComp.MoveStartPos != velocityComp.TargetPos;
                if(isMoveable)
                {
                    if(velocityComp.StepCostTime == 0)
                    {
                        var energyCost = velocityComp.EnergyCostPerSecend;
                        var isEnergyEnough = energyComp.TryCostEnerge(energyCost, 0, true);
                    }
                    velocityComp.IsIdling = false;
                    var duration = 1.0f / velocityComp.Speed.GetFinalValue(time.FrameCount);
                    velocityComp.AddStepCostTime(time.DeltaTime);
                    var currentTime = velocityComp.StepCostTime;
                    var progress = currentTime / duration;
                    bool isArrive = progress >= 1.0f;
                    progress = Mathf.Clamp01(progress);
                    var progress_pos = jumperComp.JumpCurve.Evaluate(progress);
                    var pos = Vector3.Lerp(velocityComp.MoveStartPos, velocityComp.TargetPos, progress_pos);
                    transComp.Position = pos;
                    if(isArrive)
                    {
                        velocityComp.MoveStartPos = velocityComp.TargetPos;
                        velocityComp.ClearStepCostTime();
                    }
                }
                else
                {
                    velocityComp.IsIdling = true;
                }               

            }
        }
    }
}
