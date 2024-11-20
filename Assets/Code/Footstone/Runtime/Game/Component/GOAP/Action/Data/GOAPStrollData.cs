using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPStrollData")]
    public class GOAPStrollData : GOAPActionData
    {
        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPStrollComponent>(entity);
        }

        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPStrollComponent>(entity, null);
        }
    }

}



