using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //暂定就发起一次攻击
    public class GOAPAttackPlayerProcessor : SimpleGameEntityProcessor<GOAPAttackPlayerComponent, GOAPAgentComponent, AttackComponent, ActionMaskComponent, RotateComponent>
    {
        PlayerProcessor playerProcessor;

        public GOAPAttackPlayerProcessor() : base()
        {
            Order = ProcessorOrder.AttackCommandInput;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            playerProcessor = GetProcessor<PlayerProcessor>();
        }

        public override void Update(GameTime time)
        {
            
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                bool isFinsh = false;
                var actionComp = kvp.Value.Component1;
                var agentComp = kvp.Value.Component2;
                var attackComp = kvp.Value.Component3;
                var actionMaskComp = kvp.Value.Component4;
                var isAttackbale = AttackUtil.Attackable(attackComp, actionMaskComp);
                if (isAttackbale)
                {
                    if (actionComp.IsWaitAttackFinsh) isFinsh = true;
                    else
                    {
                        var playerPosition = playerProcessor.Target.Position;
                        var currentPosition = kvp.Key.Entity.Transform.Position;
                        var rotateComp = kvp.Value.Component5;
                        var attackDir = AttackUtil.CacluteAttackDir(currentPosition, playerPosition);
                        AttackUtil.StartAttackNoCheck(attackComp, actionMaskComp, rotateComp, attackDir, actionComp.AttackIndex);
                        actionComp.IsWaitAttackFinsh = true;
                    }
                }
                
                if(isFinsh)
                {
                    //cmd.RemoveEntityComponent(actionComp);
                    agentComp.Task.FinshCurrentAction(true);
                }

            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        protected override void OnEntityComponentRemoved(Entity entity, GOAPAttackPlayerComponent component, GameData<GOAPAttackPlayerComponent, GOAPAgentComponent, AttackComponent, ActionMaskComponent, RotateComponent> data)
        {
            component.Reset();
        }
    }
}
