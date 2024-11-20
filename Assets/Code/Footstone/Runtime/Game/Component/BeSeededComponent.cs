using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //被播种
    [DefaultEntityComponentProcessor(typeof(BeSeededProcesor))]
    public class BeSeededComponent : EntityComponent 
    {
        public string Plant;
    }
}
