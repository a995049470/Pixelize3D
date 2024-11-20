using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class FinishBePickedUpProcessor : SimpleGameEntityProcessor<PickLabelComponent>
    {
        public FinishBePickedUpProcessor() : base()
        {
            Order = ProcessorOrder.FinshTakeEffect;
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var entity = kvp.Key.Entity;
                var pickLabelComp = kvp.Value.Component1;
                if (pickLabelComp.IsEffect)
                {
                    if(pickLabelComp.CacheComponents != null)
                    {
                        cmd.MoveEntityComponents(pickLabelComp.CacheComponents, entity);
                        pickLabelComp.CacheComponents = null;
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

    
}
