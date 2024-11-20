using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 掉落物销毁
    /// </summary>
    public class AutoRecyclePickableProcessor : SimpleGameEntityProcessor<PickLabelComponent, DeadComponent, AutoRecycleComponent>
    {
        public AutoRecyclePickableProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var pickLabelComp = kvp.Value.Component1;
                var deadComp = kvp.Value.Component2;
                var autoRecycleComp = kvp.Value.Component3;
                //不需要关注ownerComp是否有target
                if(pickLabelComp.IsEffect)
                {
                    var entity = deadComp.Entity;
                    if(deadComp.DeadTime == 0)
                    {
                        if(!string.IsNullOrEmpty(deadComp.ParticleName))
                        cmd.InstantiateParticle(deadComp.ParticleName, entity.Transform.Position);
                    }
                    if(deadComp.WillDesotry(time.DeltaTime))
                    {
                        cmd.RemoveEntityComponent(pickLabelComp);
                        autoRecycleComp.RecycleEntity(cmd, entity);
                    }
                    deadComp.DeadTime += time.DeltaTime;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);

        }
    }
}
