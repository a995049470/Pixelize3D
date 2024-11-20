using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/GOAP/Action/GOAPFaceFixedDirectiondData")]
    public class GOAPFaceFixedDirectiondData : GOAPActionData
    {
        public Vector3 FixedForward = new Vector3(0, 0, 1);
        public string TipKey = "";

        
        public override void DisableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.RemoveEntityComponent<GOAPFaceFixedDirectionComponent>(entity);
            cmd.RemoveEntityComponent<ShowTipComponent>(entity);
        }

        public override void EnableGOAPActionComponent(CommandBuffer cmd, Entity entity)
        {
            cmd.AddEntityComponent<GOAPFaceFixedDirectionComponent>(entity, comp =>
            {
                comp.FixedForward = FixedForward;
            });
            cmd.AddEntityComponent<ShowTipComponent>(entity, comp =>
            {
                comp.TipKey = TipKey;
                comp.IsWaitPlayerRespond = true;
            });
        }
    }

}



