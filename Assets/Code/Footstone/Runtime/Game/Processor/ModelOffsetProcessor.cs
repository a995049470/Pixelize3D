using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class ModelOffsetProcessor : SimpleGameEntityProcessor<ModelOffsetComponent>
    {
        private HashSet<ModelOffsetComponent> newModelOffsetComponents = new();
        public override void Update(GameTime time)
        {
            if(newModelOffsetComponents.Count > 0)
            {
                foreach (var comp in newModelOffsetComponents)
                {
                    var modelPoint = comp.ModelPointReference.Component;
                    var z = Mathf.RoundToInt(PositionUtil.CorrectPosition(comp.Entity.Transform.Position).z);
                    var dir = Vector3.right;
                    var angle = random.Next(0, 360);
                    dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                    var localPos = comp.Offset * dir;
                    modelPoint.localPosition = localPos;
                }
                newModelOffsetComponents.Clear();
            }

        }

        protected override void OnEntityComponentAdding(Entity entity, ModelOffsetComponent component, GameData<ModelOffsetComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            newModelOffsetComponents.Add(component);
        }

        protected override void OnEntityComponentRemoved(Entity entity, ModelOffsetComponent component, GameData<ModelOffsetComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            newModelOffsetComponents.Remove(component);
        }
    }
}
