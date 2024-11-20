using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Collection
{
    [System.Serializable]
    public class SerializableStack<T> : Stack<T>
    {
        [SerializeField][HideInInspector]
        private List<T> cache = new();
        public void OnBeforeSave()
        {
            cache.Clear();
            cache.AddRange(this);
        }

        public void OnAfterLoad()
        {
            this.Clear();
            for (int i = cache.Count - 1; i >= 0 ; i--)
            {
                this.Push(cache[i]);
            }
            cache.Clear();
        }
    }
}
