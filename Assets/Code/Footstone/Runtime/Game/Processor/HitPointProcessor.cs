using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class HitPointData
    {
        public HitPointComponent HitPoint;
    }

    public class HitPointProcessor : GameEntityProcessor<HitPointComponent, HitPointData>
    {
        protected override HitPointData GenerateComponentData(Entity entity,  HitPointComponent component)
        {
            return new HitPointData() 
            {
                HitPoint = component
            };
        }
    }
}
