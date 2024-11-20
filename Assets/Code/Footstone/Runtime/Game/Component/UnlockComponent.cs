using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(InteractionUnlockProcessor))]
    public class UnlockComponent : EntityComponent
    {
        public string KeyName = "";
    }

}
