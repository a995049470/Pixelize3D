using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class SlowBuffEffectProcessor : SimpleGameEntityProcessor<SlowBuffComponent, VelocityComponent, RotateComponent, AttackComponent>
    {
        public SlowBuffEffectProcessor() : base()
        {
            Order = ProcessorOrder.NormalBuffEffect;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var buffComp = kvp.Value.Component1;
                var velocityComp = kvp.Value.Component2;
                var rotateComp = kvp.Value.Component3;
                var attackComp = kvp.Value.Component4;
                bool isComplete = buffComp.TryCompleteSlow(time.DeltaTime, time.FrameCount, out var slowPercentage);
                if(!isComplete)
                {
                    slowPercentage *= -1;
                    velocityComp.Speed.ReceiveTempPercentPower(time.FrameCount, slowPercentage);
                    rotateComp.RotateSpeed.ReceiveTempPercentPower(time.FrameCount, slowPercentage);
                    attackComp.AttackSpeed.ReceiveTempPercentPower(time.FrameCount, slowPercentage);
                }
                else
                {
                    cmd.RemoveEntityComponent(buffComp);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
