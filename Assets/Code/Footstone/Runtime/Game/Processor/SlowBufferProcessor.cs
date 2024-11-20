using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AddSlowBuffProcessor : SimpleGameEntityProcessor<SlowBufferComponent, PowerReceiverComponent>
    {
        public AddSlowBuffProcessor() : base()
        {
            Order = ProcessorOrder.AddBuff;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var bufferComp = kvp.Value.Component1;
                cmd.AddEntityComponent<SlowBuffComponent>(bufferComp.Entity, comp =>
                {
                    comp.AddSolwBuff(0, bufferComp.SlowPercentage, bufferComp.SlowDuration);
                });
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
