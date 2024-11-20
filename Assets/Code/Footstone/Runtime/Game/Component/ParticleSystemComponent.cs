using Lost.Runtime.Footstone.Core;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{

    [DisallowMultipleComponent]
    public class ParticleSystemComponent : UnityComponent<ParticleSystem>
    {
        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            Component.Clear(true);
        }
    }
}
