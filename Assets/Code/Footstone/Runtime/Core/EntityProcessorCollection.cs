using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Lost.Runtime.Footstone.Collection;

namespace Lost.Runtime.Footstone.Core
{

    /// <summary>
    /// Ordered collection of <see cref="EntityProcessor"/> based on the <see cref="EntityProcessor.Order"/> property.
    /// </summary>
    /// <seealso cref="Stride.Core.Collections.OrderedCollection{Stride.Engine.EntityProcessor}" />
    public class EntityProcessorCollection : OrderedCollection<EntityProcessor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityProcessorCollection"/> class.
        /// </summary>
        public EntityProcessorCollection() : this(4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityProcessorCollection"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public EntityProcessorCollection(int capacity) : base(EntityProcessorComparer.Default, capacity)
        {
        }

        /// <summary>
        /// Gets the first processor of the type T.
        /// </summary>
        /// <typeparam name="T">Type of the processor</typeparam>
        /// <returns>The first processor of type T or <c>null</c> if not found.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>() where T : EntityProcessor
        {
            for (int i = 0; i < this.Count; i++)
            {
                var system = this[i] as T;
                if (system != null)
                    return system;
            }

            return null;
        }

        /// <summary>
        /// Internal comparer for <see cref="EntityProcessor"/>
        /// </summary>
        private class EntityProcessorComparer : Comparer<EntityProcessor>
        {
            public static new readonly EntityProcessorComparer Default = new EntityProcessorComparer();

            public override int Compare(EntityProcessor x, EntityProcessor y)
            {
                return x.Order.CompareTo(y.Order);
            }
        }
    }
}

