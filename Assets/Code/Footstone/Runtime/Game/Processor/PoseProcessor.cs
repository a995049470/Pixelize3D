

using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class PoseData
    {
        public PoseComponent PoseComponent;
    }

    //暂时不需要播放器
    public class PoseProcessor : GameEntityProcessor<PoseComponent, PoseData>
    {

        protected override PoseData GenerateComponentData( Entity entity,  PoseComponent component)
        {
            return new PoseData
            {
                PoseComponent = component,
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity,  PoseComponent component,  PoseData associatedData)
        {
            return component == associatedData.PoseComponent;
        }

        protected override void OnEntityComponentAdding(Entity entity,  PoseComponent component,  PoseData data)
        {
            
        }

        protected override void OnEntityComponentRemoved(Entity entity,  PoseComponent component,  PoseData data)
        {
            

        }

        
    }
}
