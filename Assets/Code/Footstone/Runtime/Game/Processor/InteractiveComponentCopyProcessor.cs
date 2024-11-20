namespace Lost.Runtime.Footstone.Game
{
    public class InteractiveComponentCopyProcessor : EntityComponentCopyProcessor<InteractiveComponent>
    {
        public override void CopyTo(InteractiveComponent src, InteractiveComponent dst)
        {
            dst.TriggerFlag = src.TriggerFlag;
        }
    }
}
