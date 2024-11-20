using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class BeDyingRecycleProcessor : SimpleGameEntityProcessor<BeDyingComponent, AutoRecycleComponent>
    {
        public BeDyingRecycleProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var beDyingComp = kvp.Value.Component1;
                var autoRecycleComp = kvp.Value.Component2;
                if(beDyingComp.IsWillDie)
                {
                    autoRecycleComp.Recycle(cmd);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

    }
}
