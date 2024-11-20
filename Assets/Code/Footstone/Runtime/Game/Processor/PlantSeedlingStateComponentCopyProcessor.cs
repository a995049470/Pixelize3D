namespace Lost.Runtime.Footstone.Game
{
    public class PlantSeedlingStateComponentCopyProcessor : EntityComponentCopyProcessor<PlantSeedlingStateComponent>
    {
        public override void CopyTo(PlantSeedlingStateComponent src, PlantSeedlingStateComponent dst)
        {
           dst.Key = src.Key;
           dst.TargetPower = src.TargetPower;
        }
    }
}
