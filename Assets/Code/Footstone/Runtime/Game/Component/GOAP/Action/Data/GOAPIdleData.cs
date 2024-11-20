using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPIdleData")]
    public class GOAPIdleData : GOAPActionData
    {
        public Vector2 RotateIntervalRange = new Vector2(2.0f, 5.0f);

        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPIdleComponent>(entity);
        }

        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPIdleComponent>(entity, comp =>
            {
                comp.RotateIntervalRange = RotateIntervalRange;
            });
        }
    }

}



