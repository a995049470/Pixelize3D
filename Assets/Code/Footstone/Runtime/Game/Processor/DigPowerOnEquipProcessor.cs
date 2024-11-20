using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class DigPowerOnEquipProcessor : SimpleGameEntityProcessor<DigPowerComponent, PowerReceiverComponent>
    {   
        public DigPowerOnEquipProcessor() : base()
        {
            Order = ProcessorOrder.TakeEffectOnEquiping;
        }
        
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var powerComp = kvp.Value.Component1;
                if(powerComp.IsEquipping)
                {
                    var entity = kvp.Key.Entity;
                    cmd.AddEntityComponent<DigComponent>(entity, comp => 
                    {
                        comp.Receicver.ReceivePower(powerComp.PowerUID);
                    });
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }

}
