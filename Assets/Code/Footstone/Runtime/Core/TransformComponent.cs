using UnityEngine;
using Lost.Runtime.Footstone.Collection;
namespace Lost.Runtime.Footstone.Core
{
    [DisallowMultipleComponent]
    public sealed class TransformComponent : EntityComponent
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
        
        public Vector3 LocalScale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Quaternion LocalRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }

        public Vector3 Forward
        {
            get => transform.forward;
        }

        public Vector3 Right
        {
            get => transform.right;
        }

        public Vector3 Up
        {
            get => transform.up;
        }

        //public Transform Transform { get => this.transform; }
        //TODO:父物体？
        public Transform Parent
        {
            get => this.transform.parent;
            set => this.transform.SetParent(value);
        }

        

        private TransformChildrenCollection children = new();
        public FastCollection<TransformComponent> Children { get => children; }

        protected override void OnEnable()
        {
            base.OnEnable();
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (cacheEntity != null && cacheEntity.Transform != this)
                {
                    if (cacheEntity.Transform != null) DestroyImmediate(cacheEntity.Transform);
                    cacheEntity.Transform = this;
                }
            }
#endif
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                //var transform = this.transform.parent?.GetComponent<TransformComponent>();
                //this.Parent = transform;
            }
        }

        public void SetAsLastSibling()
        {
            this.transform.SetAsLastSibling();
        }

        public void SetAsFirstSibling()
        {
            this.transform.SetAsFirstSibling();
        }

        public void SetSiblingIndex(int index)
        {
            this.transform.SetSiblingIndex(index);
        }
    }
}



