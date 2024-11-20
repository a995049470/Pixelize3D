using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(PlayerElementProcessor))]
    [DefaultEntityComponentProcessor(typeof(MapObjectElementProcessor))]
    [DefaultEntityComponentProcessor(typeof(LevelDoorElementProcessor))]
    public class MapElementComponent : EntityComponent
    {
        public string IconKey = "";
        public int Layer = 0;   
    }

}



