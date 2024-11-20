using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Collection
{   
    [System.Serializable]
    public class SerializableDictionary<K, V> : Dictionary<K, V>
    {
        [SerializeField][HideInInspector]
        private List<K> cache_keys = new();
        [SerializeField][HideInInspector]
        private List<V> cache_values = new();
        public void OnBeforeSave()
        {
            cache_keys.Clear();
            cache_keys.AddRange(this.Keys);
            cache_values.Clear();
            cache_values.AddRange(this.Values);
        }

        public void OnAfterLoad()
        {
            this.Clear();
            var count = cache_keys.Count;
            for (int i = 0; i < count; i++)
            {
                this[cache_keys[i]] = cache_values[i];
            }
            cache_keys.Clear();
            cache_values.Clear();
        }
    }
}
