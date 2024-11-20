using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FinishBuffProcessor : SimpleGameEntityProcessor<BufferComponent, BufferLabelComponent>
    {
        public FinishBuffProcessor() : base()
        {
            Order = ProcessorOrder.FinishBuff;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var bufferComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                if(bufferComp.IsEffect)
                {
                    cmd.RemoveEntityComponent(ownerComp);
                    cmd.MoveEntityComponents(bufferComp.CacheComponents, bufferComp.Entity);
                    bufferComp.CacheComponents = null;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
