using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(CameraFollowPlayerProcessor))]
    public class CameraFollowPlayerComponent : EntityComponent
    {
        public Vector3 TargetOffset;
        public float Distance = 5;
        public float SmoothTime = 1;
        public float MaxSpeed = 20;
        [HideInInspector]
        public Vector3 CurrentVelocity;
    }

}
