using Lost.Runtime.Footstone.Core;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// OnEnable时清理粒子
    /// </summary>
    public class ParticleSystemClearComponent : UnityComponent<ParticleSystem>
    {
        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            Component.Clear(true);
        }
    }
}
