using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class DeadRecycleProcessor : SimpleGameEntityProcessor<DeadComponent, HitPointComponent, AutoRecycleComponent>
    {
        public DeadRecycleProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
           foreach (var kvp in ComponentDatas)
           {
                var hpComp = kvp.Value.Component2;
                var deadComp = kvp.Value.Component1;
                var recycleComp = kvp.Value.Component3;
                var entity = recycleComp.Entity;
                if(hpComp.IsDead)
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
