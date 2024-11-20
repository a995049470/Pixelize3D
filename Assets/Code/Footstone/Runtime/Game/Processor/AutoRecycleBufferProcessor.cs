using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AutoRecycleBufferProcessor : SimpleGameEntityProcessor<BufferComponent, AutoRecycleComponent>
    {
        public AutoRecycleBufferProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
           var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var bufferComp = kvp.Value.Component1;
                if(bufferComp.IsEffect)
                {
                    var recycleComp = kvp.Value.Component2;
                    recycleComp.RecycleEntity(cmd, bufferComp.Entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
