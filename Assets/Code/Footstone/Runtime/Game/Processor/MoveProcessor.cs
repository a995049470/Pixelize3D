using Lost.Runtime.Footstone.Core;
using System;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{


    /// <summary>
    /// 最终处理移动的类
    /// </summary>
    public class MoveProcessor : SimpleGameEntityProcessor<VelocityComponent, ActionMaskComponent, EnergyComponent, WalkerComponent>
    {

        public MoveProcessor() : base()
        {
            Order = ProcessorOrder.Move; 
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            float deltaTime = (float)time.DeltaTime;
            foreach (var kvp in ComponentDatas)
            {
                var data = kvp.Value;
                var velocityComp = data.Component1;
                var actionMaskComp = data.Component2;
                var transComp = kvp.Key.Entity.Transform;
                var direction = velocityComp.TargetPos - velocityComp.MoveStartPos;
                velocityComp.Direction = direction;
                bool isMoveable = actionMaskComp.IsActionEnable(ActionFlag.Move);

                if(isMoveable)
                {
                    if (direction != Vector3.zero)
                    {
                        var energyComp = kvp.Value.Component3;
                        velocityComp.IsIdling = false;
                        var originPos = transComp.Position;
                        direction = velocityComp.TargetPos - originPos;
                        var speed = velocityComp.Speed.GetFinalValue(time.FrameCount);
                        var isArrive = direction.magnitude <= speed * deltaTime;
                        direction.Normalize();
                        var energyCost = velocityComp.EnergyCostPerSecend * deltaTime;
                        var isEnergyEnough = energyComp.TryCostEnerge(energyCost, 0, true);
                        var speedParameter = isEnergyEnough ? 1 : velocityComp.EnergyNotEnoughSpeedParameter;
                        var targetPos = originPos + speed * deltaTime * speedParameter * direction;
                        if (isArrive)
                        {
                            velocityComp.MoveStartPos = velocityComp.TargetPos;
                            velocityComp.ClearStepCostTime();
                        }
                        else
                        {
                            velocityComp.AddStepCostTime(deltaTime);
                        }
                        transComp.Position = targetPos;
                    }
                    else if(!velocityComp.IsIdling)
                    {
                        //静止状态时清理额外的位移
                        velocityComp.BackToTargetPos();
                        velocityComp.IsIdling = true;
                    }
                }
            };
        }



    }
}
