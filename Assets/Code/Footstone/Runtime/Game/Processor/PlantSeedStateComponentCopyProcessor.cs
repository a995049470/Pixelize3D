namespace Lost.Runtime.Footstone.Game
{

    public class PlantSeedStateComponentCopyProcessor : EntityComponentCopyProcessor<PlantSeedStateComponent>
    {
        public override void CopyTo(PlantSeedStateComponent src, PlantSeedStateComponent dst)
        {
            dst.Key = src.Key;
            dst.TargetPower = src.TargetPower;
        }
    }
}
