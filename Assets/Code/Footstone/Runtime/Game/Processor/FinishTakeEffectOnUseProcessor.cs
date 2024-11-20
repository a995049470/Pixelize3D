using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class FinishTakeEffectOnUseProcessor : SimpleGameEntityProcessor<UseLabelComponent>
    {
        public FinishTakeEffectOnUseProcessor() : base()
        {
            Order = ProcessorOrder.FinshTakeEffect;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var entity = kvp.Key.Entity;
                var usebaleComp = kvp.Value.Component1;
                //var ownerComp = kvp.Value.Component2;
                if (usebaleComp.IsEffect)
                {
                    //cmd.RemoveEntityComponent(ownerComp);
                    cmd.MoveEntityComponents(usebaleComp.CacheComponents, entity);
                    usebaleComp.CacheComponents = null;
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

    
}
