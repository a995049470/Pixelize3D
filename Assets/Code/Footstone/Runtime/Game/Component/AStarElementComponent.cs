using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public enum AStarElementFlag
    {
        Way = 1 << 0,
        Barrier = 1 << 1,
    }

    [DefaultEntityComponentProcessor(typeof(AStarProcessor))]
    public class AStarElementComponent : EntityComponent
    {
        public AStarElementFlag ElementFlag = AStarElementFlag.Barrier;
        [HideInInspector]
        public bool IsFixed = true;
        [HideInInspector]
        public Vector3 LastPosition;

        
    }
}



