using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class AutoRecycleBulletComponent : SimpleGameEntityProcessor<BulletComponent, AutoRecycleComponent>
    {
        public AutoRecycleBulletComponent() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var bulletComp = kvp.Value.Component1;
                var autoRecycleComp = kvp.Value.Component2;
                
                if(bulletComp.IsDead)
                {
                    var entity = bulletComp.Entity;
                    autoRecycleComp.RecycleEntity(cmd, entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);

        }
    }
}
