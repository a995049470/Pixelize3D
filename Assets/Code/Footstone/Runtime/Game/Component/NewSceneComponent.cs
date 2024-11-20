using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(NewSceneTriggerProcessor))]
    [DefaultEntityComponentProcessor(typeof(NewSceneCollectionProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractionNewSceneProcessor))]
    public class NewSceneComponent : EntityComponent
    {
        public bool IsDestoryCurrentScene = false;
        public string Scene = "";
    }

}
