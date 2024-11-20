using Lost.Runtime.Footstone.Collection;

namespace Lost.Runtime.Footstone.Core
{
    public sealed class Scene 
    {
        private Scene parent;

       

        // [DataMember(-10)]
        // [Display(Browsable = false)]
        // [NonOverridable]
        // public Guid Id { get; set; }

        /// <summary>
        /// The parent scene.
        /// </summary>
        // [HideInInspector]
        // public Scene Parent
        // {
        //     get { return parent; }
        //     set
        //     {
        //         var oldParent = Parent;
        //         if (oldParent == value)
        //             return;

        //         oldParent?.Children.Remove(this);
        //         value?.Children.Add(this);
        //     }
        // }

        /// <summary>
        /// The entities.
        /// </summary>
        public EntityCollection Entities { get; }

        // /// <summary>
        // /// The child scenes.
        // /// </summary>
        // public TrackingCollection<Scene> Children { get; }


         /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        public Scene()
        {
           
            Entities = new EntityCollection(this);
            //Children = new SceneCollection(this);
        }

        /// <summary>
        /// Updates the world transform of the scene.
        /// </summary>
        // public void UpdateWorldMatrix()
        // {
        //     UpdateWorldMatrixInternal(true);
        // }

        // internal void UpdateWorldMatrixInternal(bool isRecursive)
        // {
        //     if (parent != null)
        //     {
        //         if (isRecursive)
        //         {
        //             parent.UpdateWorldMatrixInternal(true);
        //         }

        //         WorldMatrix = parent.WorldMatrix;
        //     }
        //     else
        //     {
        //         WorldMatrix = Matrix.Identity;
        //     }

        //     WorldMatrix.TranslationVector += Offset;
        // }

        // public override string ToString()
        // {
        //     return $"Scene {Name}";
        // }
    }
}



