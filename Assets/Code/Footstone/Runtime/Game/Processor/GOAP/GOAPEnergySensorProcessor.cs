using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPEnergySensorProcessor : SimpleGameEntityProcessor<GOAPEnergySensorComponent, GOAPAgentComponent, EnergyComponent>
    {
        public GOAPEnergySensorProcessor() : base()
        {
            Order = ProcessorOrder.Sensor;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var worldStatuses = kvp.Value.Component2.WorldStatus;
                var energyComp = kvp.Value.Component3;
                worldStatuses.Set(GOAPStatusFlag.Energy, (int)energyComp.CurrentEnergy);
            }
        }

    }
}
