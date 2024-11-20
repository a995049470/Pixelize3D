using Lost.Runtime.Footstone.Collection;
using UnityEngine;
namespace Lost.Runtime.Footstone.Game
{
    public delegate void OnItemUnlock(string key);


    [System.Serializable]
    //工艺数据
    public class CraftData
    {   
        [SerializeField][HideInInspector]
        private SerializableHashSet<string> unlockItems = new();
        public event OnItemUnlock OnImteUnlocked;
        public int SelectIndex = 0;

        public void Unlock(string key)
        {
            if(unlockItems.Add(key))
            {
                OnImteUnlocked?.Invoke(key);
            }
        }

        public bool IsUnlock(string key)
        {
            return unlockItems.Contains(key);
        }

        public void OnBeforeSave()
        {
            unlockItems.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            unlockItems.OnAfterLoad();
        }
    }
}
