using Lost.Runtime.Footstone.Core;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{

    /// <summary>
    /// 拾取功能
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(PickProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerPickControllerProcessor))]
    public class PickComponent : EntityComponent
    {
        [HideInInspector]
        public bool IsPicking;

    }

}
