using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(GOAPCloseToPlayerProcessor))]
    public class GOAPCloseToPlayerComponent : EntityComponent
    {
        public float TargetDistance = 1.1f;
    }

}
