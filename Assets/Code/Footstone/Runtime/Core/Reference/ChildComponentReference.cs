using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    [System.Serializable]
    public class ChildComponentReference<T> : UnityObjectReference where T : Component
    {
        [SerializeField][HideInInspector]
        private int[] childIndices;
        public int[] ChildIndices { get => childIndices; }
        [SerializeField][HideInInspector]
        private bool isNull = true;
        private bool isOverWrite = false;
        public Transform Root { get; set; }
        private T component;
        public T Component 
        {
            get
            {
                if(!component && !isNull && !isOverWrite && Root)
                {
                    component = GetComponent();
                }
                return component;
            }
            set
            {
                if(Application.isPlaying)
                {
                    isOverWrite = true;
                }
                if(component != value)
                {
                    component = value;
                    UpdateReference();
                }
            }
        }

        private T GetComponent()
        {
            T comp = null;
            if(Root && !isNull)
            {
                var transform = Root;
                bool isSuccess = true;
                foreach (var index in childIndices)
                {
                    if(index < 0 || index >= transform.childCount)
                    {
                        isSuccess = false;
                        break;
                    }
                    transform = transform.GetChild(index);
                }
                if(isSuccess) comp = transform.GetComponent<T>();
            }
            return comp;
        }

        public override Object GetDisplayObject()
        {
            component = GetComponent();
            return component;
        }

        public override System.Type GetObjectType()
        {
            return typeof(T);
        }

        protected override void UpdateReference()
        {
            if(component)
            {
                var s = new Stack<int>();
                var transform = component.transform;
            #if UNITY_EDITOR
                if(!Root)
                {
                    Debug.LogError($"ChildComponentReference Not Set Root!");
                }
            #endif
                while (transform)
                {
                    if(transform == Root)
                    {
                        break;
                    }
                    else
                    {
                        s.Push(transform.GetSiblingIndex());
                        transform = transform.parent;
                    }
                }
                if(transform != Root)
                {
                    s.Clear();
                    component = null;
                }
                childIndices = s.ToArray();
            }
            else
            {
                childIndices = new int[0];
            }
            isNull = component == null;
        }

        public override bool UpdateUnityObject(Object o)
        {
            bool isUpdate = false;
            if(o != component)
            {
                isUpdate = true;
                component = o as T;
                UpdateReference();
            }
            return isUpdate;
        }

        public override Object GetOwner()
        {
            return Root;
        }
    }

}
