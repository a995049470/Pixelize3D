using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(MapViewerProcessor))]
    public class MapViewerComponent : EntityComponent{
        [HideInInspector]
        public Vector3 LastPosition = Vector3.one * float.MaxValue;
    }

}



