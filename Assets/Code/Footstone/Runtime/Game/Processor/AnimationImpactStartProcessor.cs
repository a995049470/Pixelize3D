using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AnimationImpactStartProcessor : SimpleGameEntityProcessor<AnimationImpactEventComponent, AttackComponent, VelocityComponent>
    {
        private VelocityProcessor velocityProcessor;
        private AStarProcessor aStarProcessor;
        private PlotProcessor plotProcessor;

        public AnimationImpactStartProcessor() : base()
        {
            Order = ProcessorOrder.ImpactStart;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            velocityProcessor = GetProcessor<VelocityProcessor>();
            aStarProcessor = GetProcessor<AStarProcessor>();
            plotProcessor = GetProcessor<PlotProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var impactEventComp = kvp.Value.Component1;
                var attackComp = kvp.Value.Component2;
                var velocityComp = kvp.Value.Component3;
                Entity impactTarget = null;
                var dir = attackComp.AttackDir;
                var targetPos = PositionUtil.CorrectPosition(velocityComp.MoveStartPos + dir);
                velocityProcessor.TryFindEntity(targetPos, out impactTarget);
                if(impactTarget)
                {
                    cmd.AddEntityComponent<RepulsedComponent>(impactTarget, comp =>
                    {
                        comp.StartRepulsed(targetPos, impactEventComp.RepleSpeed, impactEventComp.RepelDis, dir);
                    });
                    impactEventComp.ImpactState = CastSpellState.Casting;
                }
                else
                {
                    // bool isVaildTargetPos = MoveUtil.IsVaildMoveTarget(targetPos, velocityProcessor, aStarProcessor, plotProcessor);
                    impactEventComp.ImpactState = CastSpellState.Success;
                }

            }
            cmd.Execute();
            commandBufferManager.Release(cmd);

        }
    }
}
