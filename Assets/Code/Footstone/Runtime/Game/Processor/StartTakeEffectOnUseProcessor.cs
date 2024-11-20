using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class StartTakeEffectOnUseProcessor : SimpleGameEntityProcessor<UseLabelComponent>
    {
        public StartTakeEffectOnUseProcessor() : base()
        {
            Order = ProcessorOrder.StartTakeEffectOnUse;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var entity = kvp.Key.Entity;
                var useLabel = kvp.Value.Component1;
                var target = useLabel.Target;
                if (target != null && !useLabel.IsEffect)
                {
                    useLabel.IsEffect = true;
                    useLabel.CacheComponents = entity.GetEntityComponents<ITakeEffectOnUse>();
                    cmd.MoveEntityComponents(useLabel.CacheComponents, target);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

    
}
