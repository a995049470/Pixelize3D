using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class DigCroplandProcessor : SimpleGameEntityProcessor<DigLabelComponent, CroplandComponent>
    {
        public DigCroplandProcessor() : base()
        {
            Order = ProcessorOrder.DiggedOut;
        }        

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var digLabelComp = kvp.Value.Component1;
                var croplandComp = kvp.Value.Component2;
                if(digLabelComp.IsDigged)
                {
                    digLabelComp.IsDigged = false;
                    croplandComp.CurrnetCroplandFlag |= CroplandFlag.Dig;  
                }
                cmd.RemoveEntityComponent(digLabelComp);
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
        
    }
}
