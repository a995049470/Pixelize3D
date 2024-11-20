using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class ParticleSystemProcessor : SimpleGameEntityProcessor<ParticleSystemComponent>
    {
        protected override void OnEntityComponentRemoved(Entity entity, ParticleSystemComponent component, GameData<ParticleSystemComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            // var main = component.Component.main;
            component.Component.time = 0;
            component.Component.Clear(true);
        }

        
    }

}
