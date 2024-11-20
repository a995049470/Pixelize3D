namespace Lost.Runtime.Footstone.Game
{
    public class PlantAdultStateComponentCopyProcessor : EntityComponentCopyProcessor<PlantAdultStateComponent>
    {
        public override void CopyTo(PlantAdultStateComponent src, PlantAdultStateComponent dst)
        {
            dst.Key = src.Key;
            dst.TargetPower = src.TargetPower;
            dst.SubStateCount = src.SubStateCount;
        }
    }
}
