namespace Lost.Runtime.Footstone.Core
{
    public struct EntityComponentEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityComponentEventArgs"/> struct.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="componentKey">The component key.</param>
        /// <param name="previousComponent">The previous component.</param>
        /// <param name="newComponent">The new component.</param>
        public EntityComponentEventArgs(Entity entity, int index, EntityComponent previousComponent, EntityComponent newComponent)
        {
            Entity = entity;
            Index = index;
            PreviousComponent = previousComponent;
            NewComponent = newComponent;
        }

        /// <summary>
        /// The entity
        /// </summary>
        public readonly Entity Entity;

        /// <summary>
        /// The index of the component in the entity
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// The previous component
        /// </summary>
        public readonly EntityComponent PreviousComponent;

        /// <summary>
        /// The new component
        /// </summary>
        public readonly EntityComponent NewComponent;
    }
}

