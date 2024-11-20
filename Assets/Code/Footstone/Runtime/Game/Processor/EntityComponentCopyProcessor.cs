using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class EntityComponentCopyProcessor<T> : SimpleGameEntityProcessor<T, CopyLabelComponent> where T : EntityComponent
    {
        public EntityComponentCopyProcessor() : base()
        {
            Order = ProcessorOrder.CopyComponent;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var srcComp = kvp.Value.Component1;
                var copyComp = kvp.Value.Component2;
                var target = copyComp.Target;
                if(target != null)
                {
                    copyComp.IsEffect = true;
                    cmd.AddEntityComponent<T>(target, dstComp => 
                    {
                        CopyTo(srcComp, dstComp);
                    });
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        public abstract void CopyTo(T src, T dst);
    } 
}
