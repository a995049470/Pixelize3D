using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AutoRecycleUseableProcessor : SimpleGameEntityProcessor<UseLabelComponent, AutoRecycleComponent>
    {
        public AutoRecycleUseableProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var useLabelComp = kvp.Value.Component1;
                if(useLabelComp.IsEffect)
                {
                    var recycleComp = kvp.Value.Component2;
                    cmd.RemoveEntityComponent(useLabelComp);
                    recycleComp.RecycleEntity(cmd, useLabelComp.Entity);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
