using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class ParticleSystemRecycleProcessor : SimpleGameEntityProcessor<AutoRecycleComponent, ParticleSystemComponent>
    {
        public ParticleSystemRecycleProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var ps = kvp.Value.Component2.Component;
                if(!ps.isPlaying)
                {
                    var recycle = kvp.Value.Component1;
                    var entity = recycle.Entity;
                    recycle.RecycleEntity(cmd, entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);

        }
    }
}
