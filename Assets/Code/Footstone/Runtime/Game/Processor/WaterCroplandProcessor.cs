using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class WaterCroplandProcessor : SimpleGameEntityProcessor<WaterLabelComponent, CroplandComponent>
    {
        public WaterCroplandProcessor() : base()
        {
            Order = ProcessorOrder.BeWatered;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var waterableComp = kvp.Value.Component1;
                if(waterableComp.IsWatered)
                {
                    waterableComp.IsWatered = false;
                    var croplandComp = kvp.Value.Component2;
                    croplandComp.CurrnetCroplandFlag |= CroplandFlag.Wet;
                }
            }
        }
    }
}
