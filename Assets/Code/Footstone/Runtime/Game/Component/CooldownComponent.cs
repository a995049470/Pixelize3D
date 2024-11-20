using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(InteractiveCooldownProcessor))]
    public class CooldownComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public int LastDay = -1;
    }

}
