using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(RandomRotateProcessor))]
    public class RandomRotateComponent : EntityComponent
    {
        public int Interval = 1;
    }
}

