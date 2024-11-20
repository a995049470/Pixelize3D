using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //传送组件
    [DefaultEntityComponentProcessor(typeof(TransportProcessor))]
    public class TransportComponent : EntityComponent
    {
        public Vector3 TargetPosition;
    }

}
