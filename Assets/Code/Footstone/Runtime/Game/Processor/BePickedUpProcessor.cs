using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class BePickedUpProcessor : SimpleGameEntityProcessor<PickLabelComponent>
    {
        public BePickedUpProcessor() : base()
        {
            Order = ProcessorOrder.BePickedUp;
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var entity = kvp.Key.Entity;
                var pickLableComp = kvp.Value.Component1;
                //var ownerComp = kvp.Value.Component2;
                var target = pickLableComp.Target;
                if (target != null && !pickLableComp.IsEffect)
                {
                    pickLableComp.IsEffect = true;
                    pickLableComp.CacheComponents = entity.GetEntityComponents<ITakeEffectOnPickFrame>();
                    cmd.MoveEntityComponents(pickLableComp.CacheComponents, target);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

    
}
