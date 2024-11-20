using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Collection
{

    [System.Serializable]
    public class SerializableHashSet<T> : HashSet<T>
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
            cache.ForEach(x => this.Add(x));
            cache.Clear();
        }
    }
}
