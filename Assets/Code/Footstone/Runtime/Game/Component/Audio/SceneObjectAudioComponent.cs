using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(SceneObjectAudioSourcePlayProcessor))]
    public class SceneObjectAudioComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public ulong AgentComponentUID = 0;
    }


}