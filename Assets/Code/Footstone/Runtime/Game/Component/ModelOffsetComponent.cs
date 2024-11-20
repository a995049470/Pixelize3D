using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(ModelOffsetProcessor))]
    public class ModelOffsetComponent : EntityComponent
    {
        public ChildComponentReference<Transform> ModelPointReference = new();
        public float Offset = 0;

        public override void UpdateReference()
        {
            base.UpdateReference();
            ModelPointReference.Root = this.transform;
        }
    }

}