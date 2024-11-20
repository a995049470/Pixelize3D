using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    [System.Serializable]
    public class TargetComponentReference<T> where T : Component
    {
        [SerializeField][HideInInspector]
        private List<int> childIndices = new();
        [SerializeField][HideInInspector]
        private ulong rootEntityUID = 0;

        private bool isDirty = true;
        private T cacheComponent;

        public void Set(Entity entity, ChildComponentReference<T> reference)
        {
            isDirty = true;
            rootEntityUID = entity.Id;
            childIndices.Clear();
            childIndices.AddRange(reference.ChildIndices);
        }

        public void Clear()
        {
            rootEntityUID = 0;
            childIndices.Clear();
            isDirty = false;
            cacheComponent = null;
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public bool TryGetComponent(SceneSystem sceneSystem, out T component)
        {
            bool isGet = false;
            component = null;
            
            if(isDirty)
            {
                if (sceneSystem.SceneInstance.TryGetEntity(rootEntityUID, out var entity))
                {
                    var transform = entity.transform;
                    bool isSuccess = true;
                    foreach (var index in childIndices)
                    {
                        if (index < 0 || index >= transform.childCount)
                        {
                            isSuccess = false;
                            break;
                        }
                        transform = transform.GetChild(index);
                    }
                    if (isSuccess) component = transform.GetComponent<T>();
                    isDirty = false;
                    cacheComponent = component;
                }
            }
            else
            {
                component = cacheComponent;
            }
            isGet = component != null;
            return isGet;
        }
    }

}
