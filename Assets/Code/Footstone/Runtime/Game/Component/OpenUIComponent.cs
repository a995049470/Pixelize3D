using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(InteractionOpenUIProcessor))]
    public class OpenUIComponent : EntityComponent
    {
        public string Key;
    }

}
