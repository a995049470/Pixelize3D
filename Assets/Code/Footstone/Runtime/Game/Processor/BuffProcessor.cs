using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class BuffProcessor : SimpleGameEntityProcessor<BufferComponent, BufferLabelComponent>
    {
        public BuffProcessor() : base()
        {
            Order = ProcessorOrder.Buff;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var bufferComp = kvp.Value.Component1;
                var ownerComp = kvp.Value.Component2;
                var target = ownerComp.Target;
                if(!bufferComp.IsEffect)
                {
                    bufferComp.IsEffect = true;
                    FastCollection<EntityComponent> components = null;
                    if(target != null)
                    {
                        var entity = bufferComp.Entity;
                        components = entity.GetEntityComponents<IBufferComponent>();
                        cmd.MoveEntityComponents(components, target);
                        bufferComp.CacheComponents = components;
                    }
                    else
                    {
                        components = new();
                    }
                    bufferComp.CacheComponents = components;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
