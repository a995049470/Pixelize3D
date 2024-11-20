using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPWaitAttackOnSleepData")]
    public class GOAPWaitAttackOnSleepData : GOAPActionData
    {
        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPWaitAttackOnSleepComponent>(entity);
        }
        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPWaitAttackOnSleepComponent>(entity, null);
            
        }
    }

}



