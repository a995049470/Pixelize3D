using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class AutoRecycleWaitPickaxeObjectProcessor : SimpleGameEntityProcessor<AutoRecycleComponent, WaitPickaxeComponent, PickaxePossibleDroppedComponent, PickaxeLabelComponent>
    {
        public AutoRecycleWaitPickaxeObjectProcessor() : base()
        {
            Order = ProcessorOrder.Recycle;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var autoRecycleComp = kvp.Value.Component1;
                var possibleDroppedComp = kvp.Value.Component3;
                var pickLabelComp = kvp.Value.Component4;
                var target = pickLabelComp.Target;
                cmd.RemoveEntityComponent(pickLabelComp);
                possibleDroppedComp.ReceiveDroppedItems(cmd, bagData, target, random);
                autoRecycleComp.RecycleEntity(cmd, autoRecycleComp.Entity);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
