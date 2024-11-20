using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AnimationImpactEndProcessor : SimpleGameEntityProcessor<AnimationImpactEventComponent, AnimationDamageEventComponent, AttackComponent, VelocityComponent>
    {
        private VelocityProcessor velocityProcessor;

        public AnimationImpactEndProcessor() : base()
        {
            Order = ProcessorOrder.ImpactEnd;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var impactEventComp = kvp.Value.Component1;
                var damageEventComp = kvp.Value.Component2;
                var attackComp = kvp.Value.Component3;
                var velocityComp = kvp.Value.Component4;
                bool isCasting = impactEventComp.ImpactState == CastSpellState.Casting;
                // if(isCasting)
                // {
                //     sceneSystem.SceneInstance.TryGetEntity(damageEventComp.TargetUIDList[0], out var impactTarget);
                //     var repulsedComp = impactTarget?.Get<RepulsedComponent>();
                //     if(repulsedComp)
                //     {
                //         bool isSuccess = repulsedComp.SuccessCasterUID == impactEventComp.Id;
                //         impactEventComp.ImpactState = isSuccess ? CastSpellState.Success : CastSpellState.Fail;
                //     }
                //     else
                //     {
                //         impactEventComp.ImpactState = CastSpellState.Success;
                //     }
                // }
                //if(impactEventComp.ImpactState == CastSpellState.Success)
                {
                    var dir = attackComp.AttackDir;
                    var dst = PositionUtil.CorrectPosition(velocityComp.MoveStartPos + dir);
                    var src = velocityComp.TargetPos;
                    if(velocityProcessor.TryMove(src, dst))
                    {
                        velocityComp.SetVaildTargetPos(dst);
                        attackComp.AttackCombo(impactEventComp.SuccessAttackIndex);   
                    }
                    else
                    {
                        attackComp.AttackCombo(impactEventComp.FailAttackIndex);   
                    }
                }
                // if(impactEventComp.ImpactState == CastSpellState.Fail)
                // {
                //     attackComp.AttackCombo(impactEventComp.FailAttackIndex); 
                // }
            }
        }
    }
}
