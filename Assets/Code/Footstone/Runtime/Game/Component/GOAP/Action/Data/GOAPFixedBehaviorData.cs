using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPFixedBehaviorData")]
    public class GOAPFixedBehaviorData : GOAPActionData
    {
        public string BehaviourKey;
        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPFixedBehaviorComponent>(entity);
        }

        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPFixedBehaviorComponent>(entity, comp =>
            {
                comp.SetBehaviourKey(BehaviourKey);
            });
        }
    }

}



