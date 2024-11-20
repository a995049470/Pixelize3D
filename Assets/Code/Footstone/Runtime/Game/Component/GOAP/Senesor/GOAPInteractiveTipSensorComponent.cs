using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(GOAPInteractiveTipSensorProcessor))]
    public class GOAPInteractiveTipSensorComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public string CacheTipKey = "";
        [UnityEngine.HideInInspector]
        public int CacheOptionIndex = -1;

        //public int FailOptionIndex = 0;
    }

}
