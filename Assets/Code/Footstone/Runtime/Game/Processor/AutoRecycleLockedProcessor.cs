using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AutoRecycleLockedProcessor : SimpleGameEntityProcessor<LockComponent, AutoRecycleComponent>
    {
        public AutoRecycleLockedProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var lockComp = kvp.Value.Component1;
                if(lockComp.IsDead)
                {
                    var recycleComp = kvp.Value.Component2;
                    recycleComp.RecycleEntity(cmd, lockComp.Entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
