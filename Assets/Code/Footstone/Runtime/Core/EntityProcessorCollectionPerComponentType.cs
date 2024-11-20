using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Core
{
    internal class EntityProcessorCollectionPerComponentType : EntityProcessorCollection
    {
        /// <summary>
        /// The processors that are depending on the component type
        /// </summary>
        public List<EntityProcessor> Dependencies;
    }
}

