using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPAttackPlayerData")]
    public class GOAPAttackPlayerData : GOAPActionData
    {
        public int AttackIndex = 0;

        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPAttackPlayerComponent>(entity);
        }

        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPAttackPlayerComponent>(entity, comp =>
            {
                comp.AttackIndex = AttackIndex;
                
            });
        }
    }
}



