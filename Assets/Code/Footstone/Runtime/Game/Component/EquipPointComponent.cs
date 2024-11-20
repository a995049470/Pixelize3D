using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class EquipPointComponent : EntityComponent
    {
        [UnityEngine.SerializeField]
        private ChildComponentReference<Transform> leftHandPointReference = new();
        [UnityEngine.SerializeField]
        private ChildComponentReference<Transform> rightHandPointReference = new();
        public ChildComponentReference<Transform> LeftHandPointReference => leftHandPointReference;
        public ChildComponentReference<Transform> RightHandPointReference => rightHandPointReference;

   
        public override void UpdateReference()
        {
            base.UpdateReference();
            leftHandPointReference.Root = this.transform;
            rightHandPointReference.Root = this.transform;
        }
    }

}



