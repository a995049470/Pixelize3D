using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(PlayerMoveControllerProcessor))]
    public class PlayerMoveControllerComponent : EntityComponent
    {
        
    }
}
