using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(GOAPPlayerSensorProcessor))]
    public class GOAPPlayerSensorComponent : EntityComponent
    {
        //获得仇恨的最大半径
        public float ReciveHatredMaxRadius = 3.0f;
        public float FOV = 90;  
        public float LoseHatredMaxRadius = 6.0f;
    }

}
