using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPCloseToPlayerData")]
    public class GOAPCloseToPlayerData : GOAPActionData
    {
        public float TargetDistance;

        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPCloseToPlayerComponent>(entity);
        }

        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPCloseToPlayerComponent>(entity, comp => 
            {
                comp.TargetDistance = TargetDistance;
            });
        }
    }
    
}



