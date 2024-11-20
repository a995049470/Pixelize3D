namespace Lost.Runtime.Footstone.Game
{
    public class PlantFruitStateComponentCopyProcessor : EntityComponentCopyProcessor<PlantFruitStateComponent>
    {
        public override void CopyTo(PlantFruitStateComponent src, PlantFruitStateComponent dst)
        {
            dst.Key = src.Key;
            dst.TargetPower = src.TargetPower;
            dst.Fruit = src.Fruit;
            dst.Count = src.Count;
            dst.TargetRefruitCount = src.TargetRefruitCount;
        }
    }
}
