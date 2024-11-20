using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class LandPlantProcessor : SimpleGameEntityProcessor<LandPlantComponent>
    {
        public LandPlantProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
        }

        public void ReceiveGrowthPower(int power)
        {
            foreach (var kvp in ComponentDatas)
            {
                kvp.Value.Component1.RemainGrowthPower += power;
            }
        }

       
    }
}
