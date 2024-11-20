using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class ModelRootComponent : EntityComponent
    {
        public ChildComponentReference<Transform> ModelRootReference = new();

        public override void UpdateReference()
        {
            base.UpdateReference();
            ModelRootReference.Root = this.transform;
        }
    }

}



