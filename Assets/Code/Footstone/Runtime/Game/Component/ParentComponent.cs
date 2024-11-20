using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(FollowParentProcessor))]
    public class ParentComponent : EntityComponent
    {
        [HideInInspector]
        public TargetComponentReference<Transform> TargetReference = new();

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            TargetReference.SetDirty();
        }
        
    }
}
