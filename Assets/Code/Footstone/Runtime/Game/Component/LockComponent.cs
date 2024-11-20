using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(LockProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractionLockedCheckProcessor))]
    [DefaultEntityComponentProcessor(typeof(AutoRecycleLockedProcessor))]
    public class LockComponent : EntityComponent
    {
        public string LockName = "";
        [HideInInspector]
        public bool IsLocked = true;
        public bool IsDead { get => !IsLocked; }

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsLocked = true;
        }
    }

}
