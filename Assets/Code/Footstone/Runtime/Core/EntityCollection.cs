using System;
using Lost.Runtime.Footstone.Collection;

namespace Lost.Runtime.Footstone.Core
{
    public class EntityCollection : TrackingCollection<Entity>
    {
        Scene scene;

        public EntityCollection(Scene sceneParam)
        {
            scene = sceneParam;
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, Entity item)
        {
            // Root entity in another scene, or child of another entity
            if (item.Scene != null)
                throw new InvalidOperationException("This entity already has a scene. Detach it first.");

            item.SceneValue = scene;
            base.InsertItem(index, item);
        }

        /// <inheritdoc/>
        protected override void RemoveItem(int index)
        {
            var item = this[index];
            if (item.SceneValue != scene)
                throw new InvalidOperationException("This entity's scene is not the expected value.");

            item.SceneValue = null;
            base.RemoveItem(index);
        }
    }
}



