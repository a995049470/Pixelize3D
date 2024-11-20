using System.Collections;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public delegate void OnGridItemDataChange(int girdPos);
    public delegate void OnItemCountChange(string itemKey, int itemCount);

    //背包数据
    [System.Serializable]
    public class BagData
    {   
        // TODO:是否应该用空去表示这个格子没有物体
        public BagItemData[] BagItemDatas;
        //背包尺寸
        public readonly int Width = 10;
        public readonly int Height = 7;
        public int GridNum => Width * Height;
        
        //快捷栏
        [SerializeField][HideInInspector]
        private int fastSelectIndex;
        public int FastSelectIndex { get => fastSelectIndex; }

        //用来加快索引的字典
        private Dictionary<string, List<BagItemData>> gridCacheDic = new();
        public event OnGridItemDataChange OnGridItemDataChanged;
        public event OnItemCountChange OnItemCountChange;
        [UnityEngine.HideInInspector]
        public bool HasInitialized = false;

        private Dictionary<string, int> cacheNumDic = new Dictionary<string, int>();


        private ResPoolManager resPoolManager;
        private UniqueIdManager uniqueIdManager;

        public BagData()
        {

        }

        public BagData(IServiceRegistry service)
        {
            Initialized(service);
            
        }   

        public void Initialized(IServiceRegistry service)
        {
            if(BagItemDatas == null || BagItemDatas.Length != Width * Height)
            {
                BagItemDatas =  new BagItemData[Width * Height];
                for (int i = 0; i < Width * Height; i++)
                {
                    BagItemDatas[i] = new();
                }
            }
            uniqueIdManager = service.GetService<UniqueIdManager>();
            resPoolManager = service.GetService<ResPoolManager>();
            ReceiveItem("weapon_dagger", 1);
        }

        public void SetFastSelectIndex(int index)
        {
            if(fastSelectIndex != index && index >=0 && index < Width)
            {
                OnGridItemDataChanged?.Invoke(fastSelectIndex);
                fastSelectIndex = index;
                OnGridItemDataChanged?.Invoke(fastSelectIndex);
            }
        }
        

        /// <summary>
        /// 获取格子上的物体数据(不要保留这个引用)
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public BagItemData GetBagGridItemData(int pos)
        {
            BagItemData gridData = null;
            if(pos >= 0 && pos < BagItemDatas.Length)
            {
                gridData = BagItemDatas[pos];
                // if(!gridData.IsVaild()) gridData = null;
            }
            return gridData;
        }

        private bool IsFastGrid(int index)
        {
            return index < Width;
        }

        public bool TrySwitchItem(int src, int dst)
        {
            bool isSuccess = true;
            if(IsFastGrid(dst))
            {
                isSuccess &= IsUseableOrEmpty(src);
            }
            if(isSuccess && IsFastGrid(src))
            {
                isSuccess &= IsUseableOrEmpty(dst);
            }
            if(isSuccess)
            {
                var srcData = BagItemDatas[src];
                var dstData = BagItemDatas[dst];
                srcData.GridIndex = dst;
                dstData.GridIndex = src;
                BagItemDatas[srcData.GridIndex] = srcData;
                BagItemDatas[dstData.GridIndex] = dstData;
                OnGridItemDataChanged?.Invoke(src);
                OnGridItemDataChanged?.Invoke(dst);
            }
            return isSuccess;
        }
        
        private bool IsUseable(string itemKey)
        {
            var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[itemKey];
            return data.ContainsKey(JsonKeys.power) && !string.IsNullOrEmpty(data[JsonKeys.power].ToString());
        }

        private bool IsUseableOrEmpty(int pos)
        {
            bool isPass = true;
            var itemData = GetBagGridItemData(pos);
            if(itemData.IsVaild())
            {
                var itemKey = itemData.ItemKey;
                var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[itemKey];
                isPass &= data.ContainsKey(JsonKeys.power) && !string.IsNullOrEmpty(data[JsonKeys.power].ToString());
            }
            return isPass;
        }

        /// <summary>
        /// 获取道具
        /// </summary>
        public void ReceiveItem(string itemKey, int num)
        {  
            if(num > 0)
            {
                OnItemCountChange?.Invoke(itemKey, num);
                var notFullGrids = GetNotFullGrids(itemKey, true);
                foreach (var grid in notFullGrids)
                {
                    var remainNum = grid.GetMaxNum() - grid.ItemCount;
                    var addNum = Mathf.Min(num, remainNum);
                    grid.ItemCount += addNum;
                    OnGridItemDataChanged?.Invoke(grid.GridIndex);
                    num -= addNum;
                    if(num == 0) break;
                }
            }
            if(num > 0)
            {
                int count = Width * Height;
                bool isUseable = IsUseable(itemKey);
                int startIndex = isUseable ? 0 : Width;
                for (int i = 0; i < count; i++)
                {
                    var gridIndex = (i + startIndex) % count;
                    var gird = BagItemDatas[gridIndex];
                    if(!gird.IsVaild())
                    {
                        gird.Initialize(gridIndex, itemKey, uniqueIdManager, resPoolManager);
                        var addNum = Mathf.Min(gird.GetMaxNum(), num);
                        num -= addNum;
                        gird.ItemCount += addNum;
                        OnGridItemDataChanged?.Invoke(gird.GridIndex);
                        //BagItemDatas[i] = gird;
                        AddGridCache(gird);
                        if(num == 0) break;
                    }   
                }
            }
        }   

        public bool IsBagGridItemNumEnough(int gridIndex, ulong uid, int num)
        {
            bool isSuccess = num == 0;
            if(num > 0)
            {   
                var itemData = BagItemDatas[gridIndex];
                isSuccess = itemData.ItemCount >= num && uid == itemData.UID;
            }
            return isSuccess;
        }

        public bool IsItemNumEnough(string item, int num)
        {
            return GetItemCount(item) >= num;
        }
        
        public bool IsItemNumEnough(IEnumerable<ItemInfo> itemInfos)
        {
            bool isEnough = true;
            //可能存在重复
            cacheNumDic.Clear();
            foreach (var info in itemInfos)
            {
                cacheNumDic.TryGetValue(info.Name, out var total);
                total += info.Count;
                cacheNumDic[info.Name] = total;
            }
            foreach (var kvp in cacheNumDic)
            {
                if(GetItemCount(kvp.Key) < kvp.Value)
                {
                    isEnough = false;
                    break;
                }
            }
            return isEnough;
        }


       
        /// <summary>
        //尝试失去物体 若为true 会直接直接消耗物品
        /// </summary>
        public bool TryLoseItem(int gridIndex, ulong uid, int num)
        {
            bool isSuccess = num == 0;
            if(num > 0)
            {
                var itemData = BagItemDatas[gridIndex];
                if(itemData.ItemCount >= num && uid == itemData.UID)
                {
                    isSuccess = true;
                    itemData.ItemCount -= num;
                    OnGridItemDataChanged?.Invoke(gridIndex);
                    OnItemCountChange?.Invoke(itemData.ItemKey, -num);
                    if(itemData.ItemCount == 0)
                    {
                        //BagItemDatas[itemData.GridIndex] = null;
                        RemoveGridCache(itemData);
                        itemData.Release(uniqueIdManager);
                    }
                }
                
            }
            return isSuccess;
        }

        public bool TryLoseItem(string itemKey, int num)
        {
            bool isSuccess = GetItemCount(itemKey) >= num;
            if(isSuccess)
            {
                LoseItem(itemKey, num);
            }
            return isSuccess;
        }
        
        /// <summary>
        /// 丢失物品，不会检查身上的物体是否足够，即使不足也会全丢
        /// </summary>
        public void LoseItem(string itemKey, int num)
        {
            if(num > 0)
            {
                var originNum = num;
                var itemGrids = GetItemGrids(itemKey, true);
                for (int i = itemGrids.Count - 1; i >= 0 ; i--)
                {
                    var grid = itemGrids[i];
                    var cutNum = Mathf.Min(grid.ItemCount, num);
                    grid.ItemCount -= cutNum;
                    OnGridItemDataChanged?.Invoke(grid.GridIndex);
                    num -= cutNum;
                    if(grid.ItemCount == 0)
                    {
                        //BagItemDatas[grid.GridIndex] = null;
                        RemoveGridCache(grid);
                        grid.Release(uniqueIdManager);
                    }
                    if(num == 0) break;
                }
                var loseCount = originNum - num;
                if(loseCount > 0)
                {
                    OnItemCountChange?.Invoke(itemKey, -loseCount);
                }
            }
        }

        public int GetItemCount(string itemKey)
        {
            int itemCount = 0;  
            if(gridCacheDic.TryGetValue(itemKey, out var list))
            {
                list.ForEach(grid => itemCount += grid.ItemCount);
            }
            return itemCount;
        }

        private List<BagItemData> GetNotFullGrids(string itemKey, bool isSort)
        {
            var notFullGrids = new List<BagItemData>();
            if(gridCacheDic.TryGetValue(itemKey, out var list))
            {
                bool isAscendingOrder = true;
                var lastPos = -1;
                foreach (var gridData in list)
                {
                    if(!gridData.IsFull()) 
                    {
                        isAscendingOrder &= gridData.GridIndex > lastPos;
                        lastPos = gridData.GridIndex;
                        notFullGrids.Add(gridData);
                    }
                }
                if(!isAscendingOrder && isSort)
                {
                    notFullGrids.Sort((x, y) => x.GridIndex - y.GridIndex);
                    list.Sort((x, y) => x.GridIndex - y.GridIndex);
                }
            }
            return notFullGrids;
        }

        private List<BagItemData> GetItemGrids(string itemKey, bool isSort)
        {
            var itemGrids = new List<BagItemData>();
            if(gridCacheDic.TryGetValue(itemKey, out var list))
            {
                if (isSort)
                {
                    bool isAscendingOrder = true;
                    var lastPos = -1;
                    foreach (var gridData in list)
                    {
                        isAscendingOrder &= gridData.GridIndex > lastPos;
                        lastPos = gridData.GridIndex;
                    }
                    if (!isAscendingOrder)
                    {
                        list.Sort((x, y) => x.GridIndex - y.GridIndex);
                    }
                }
                itemGrids.AddRange(list);
            }
            return itemGrids;
        }
        

        public void AddGridCache(BagItemData gridData)
        {
            if(!gridCacheDic.TryGetValue(gridData.ItemKey, out var list))
            {
                list = new();
                gridCacheDic[gridData.ItemKey] = list;
            }
            list.Add(gridData);
        }

        public void RemoveGridCache(BagItemData gridData)
        {
            if(gridCacheDic.TryGetValue(gridData.ItemKey, out var list))
            {   
                list.Remove(gridData);
                if(list.Count == 0) gridCacheDic.Remove(gridData.ItemKey);
            }
        }

        public void ClearGridCache()
        {
            gridCacheDic.Clear();
        }


        public void AfterLoad(IServiceRegistry service)
        {
            resPoolManager = service.GetService<ResPoolManager>();
            uniqueIdManager = service.GetService<UniqueIdManager>();
            ClearGridCache();
            foreach (var bagGrid in BagItemDatas)
            {
                if(bagGrid != null)
                {
                    bagGrid.OnLoad(resPoolManager);
                    if(bagGrid.IsVaild())
                    {
                        AddGridCache(bagGrid);
                    }
                }
            }
        }


        
    }
}
