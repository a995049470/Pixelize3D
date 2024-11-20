using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class HealProcessor : SimpleGameEntityProcessor<HealPowerComponent,  HurtComponent, PowerReceiverComponent>
    {
        public HealProcessor() : base()
        {
            Order = ProcessorOrder.Heal;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var hurtComp = kvp.Value.Component2;
                var healComp = kvp.Value.Component1;
                hurtComp.Heal += healComp.Value;
                UnityEngine.Debug.Log($"施加了{healComp.Value}点治疗");
            }
        
        }
    }
}
