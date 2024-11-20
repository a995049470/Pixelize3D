using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(CameraProcessor))]
    public class CameraComponent : UnityComponent<Camera>
    {
        
    }

}
