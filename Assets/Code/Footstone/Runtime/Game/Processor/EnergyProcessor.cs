using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class EnergyProcessor : SimpleGameEntityProcessor<EnergyComponent>
    {
        public EnergyProcessor() : base()
        {
            Order = ProcessorOrder.NaturalRecoveryEnergy;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                //自然回能
                kvp.Value.Component1.NaturalRecoveryEnergy(time.DeltaTime);
            }
        }
    }
}
