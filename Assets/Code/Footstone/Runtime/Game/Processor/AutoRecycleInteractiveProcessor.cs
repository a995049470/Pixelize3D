using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AutoRecycleInteractiveProcessor : SimpleGameEntityProcessor<DeadComponent, InteractiveComponent, AutoRecycleComponent>
    {
        public AutoRecycleInteractiveProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }
        
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
           foreach (var kvp in ComponentDatas)
           {
                
                var deadComp = kvp.Value.Component1;
                var interactionComp = kvp.Value.Component2;
                var recycleComp = kvp.Value.Component3;
                var entity = recycleComp.Entity;
                if(interactionComp.IsWaitRecycle)
                {
                    if(deadComp.DeadTime == 0)
                    {         
                        //加载特效
                        if(!string.IsNullOrEmpty(deadComp.ParticleName))
                            cmd.InstantiateParticle(deadComp.ParticleName, entity.Transform.Position);
                    }
                    if(deadComp.WillDesotry(time.DeltaTime))
                    {
                        recycleComp.RecycleEntity(cmd, entity);
                    }
                    deadComp.DeadTime += time.DeltaTime;
                }
                
           }
           cmd.Execute();
           commandBufferManager.Release(cmd);
        }
    }
}
