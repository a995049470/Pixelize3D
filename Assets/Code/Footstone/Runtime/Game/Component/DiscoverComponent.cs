using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(DiscoverProcessor))]
    public class DiscoverComponent : EntityComponent, ITakeEffectOnUse
    {
        public string Key;
    }

}
