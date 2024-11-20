using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(UniqueChestCheckProcessor))]
    [DefaultEntityComponentProcessor(typeof(UniqueChestProcessor))]
    public class UniqueChestComponent : EntityComponent
    {
        
    }

}
