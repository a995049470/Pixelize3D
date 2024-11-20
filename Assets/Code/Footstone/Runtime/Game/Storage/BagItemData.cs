using LitJson;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //背包道具数据
    [System.Serializable]
    public class BagItemData
    {
        public string ItemKey;
        public int ItemCount;
        public int GridIndex;
        //用来标志道具的唯一性。道具发生变化时也可能发生UID的变化
        public ulong UID;
        private JsonData cacheData;
        
        public BagItemData()
        {

        }

        // public ItemData(int pos, string key, UniqueIdManager uniqueIdManager, ResPoolManager resPoolManager)
        // {
           
        // }

        public void Initialize(int pos, string key, UniqueIdManager uniqueIdManager, ResPoolManager resPoolManager)
        {
            GridIndex = pos;
            ItemKey = key;
            UID = uniqueIdManager.CreateUniqueId();
            cacheData = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[key];
        }

        /// <summary>
        /// 物体发生变化的时候改变UID
        /// </summary>
        /// <param name="uniqueIdManager"></param>
        public void ResetUID(UniqueIdManager uniqueIdManager)
        {
            uniqueIdManager.RecycleUniqueId(ref UID);
            UID = uniqueIdManager.CreateUniqueId();
        }

        //物品消失时候释放
        public void Release(UniqueIdManager uniqueIdManager)
        {
            //ItemKey = "";
            cacheData = null;
            uniqueIdManager.RecycleUniqueId(ref UID);
        }
        

        private JsonData GetCacehData()
        {
            return cacheData;
        }

        public bool IsFull()
        {
            return ItemCount == GetMaxNum();
        }

        public string GetIconKey()
        {
            var data = GetCacehData();
            var iconKey = ((string)data[JsonKeys.icon]);
            return iconKey;
        }

        public string GetItemName()
        {
            var data = GetCacehData();
            var itemName = ((string)data[JsonKeys.name]);
            return itemName;
        }

        /// <summary>
        /// 获取道具描述（可能要做区分）
        /// </summary>
        public string GetDesc()
        {
            var data = GetCacehData();
            var desc = ((string)data[JsonKeys.desc_bag]);
            return desc;
        }

    
        public int GetMaxNum()
        {
            var data = GetCacehData();
            var maxNum = UnityEngine.Mathf.Max(1, (int)data[JsonKeys.maxNum]);
            return maxNum;
        }
        
        public string GetPowerEntityKey()
        {
            var data = GetCacehData();
            var key = (string)data[JsonKeys.power];
            return key;
        }

        public string GetEffectEntityKey()
        {
            var data = GetCacehData();
            var key = (string)data[JsonKeys.effect];
            return key;
        }

        public string GetEntityKey()
        {
            var data = GetCacehData();
            var key = data.ContainsKey(JsonKeys.entity) ? ((string)data[JsonKeys.entity]) : "";
            return key; 
        }

        public bool IsUseable()
        {
            var data = GetCacehData();
            return data.ContainsKey(JsonKeys.power) && !string.IsNullOrEmpty(data[JsonKeys.power].ToString());
        }

       

        public void OnLoad(ResPoolManager resPoolManager)
        {
            if(IsVaild())
            {
                cacheData = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[ItemKey];
            }
        }
        
        public bool IsVaild()
        {
            return UID != 0;
        }
        
    }
}
