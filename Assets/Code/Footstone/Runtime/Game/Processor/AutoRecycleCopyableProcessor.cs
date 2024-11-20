using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{


    public class AutoRecycleCopyableProcessor : SimpleGameEntityProcessor<CopyLabelComponent, AutoRecycleComponent>
    {
        public AutoRecycleCopyableProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var copyComp = kvp.Value.Component1;
                //var ownerComp = kvp.Value.Component2;
                var autoRecycleComp = kvp.Value.Component2;
                if(copyComp.IsEffect)
                {
                    if(copyComp.Target != null)
                    {
                        cmd.RemoveEntityComponent(copyComp);
                        autoRecycleComp.RecycleEntity(cmd, copyComp.Entity);
                    }
                }
                else
                {
                    //可能没有可以赋值组件 下次回收
                    copyComp.IsEffect = true;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
