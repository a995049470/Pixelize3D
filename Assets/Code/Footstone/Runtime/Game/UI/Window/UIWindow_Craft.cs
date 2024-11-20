using System.Collections.Generic;
using LitJson;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    
    public class UIWindow_Craft : UIWindow
    {
        private UIView_Craft view;
        private List<string> carftItemKeys = new();
        private List<UIModel_CraftGrid> craftGrids = new();
        private HashSet<int> dirtyGridIndices = new();
        private bool isDitryDesc;
        private List<UIModel_CraftCost> craftCosts = new();
        private CraftProcessor craftProcessor;
        private CraftData craftData { get => craftProcessor.SingleComponent.Data; }

        public override int Orderer => GameConstant.Craft;

        public UIWindow_Craft(UIView view) : base(view)
        {

        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            craftProcessor = GetProcessor<CraftProcessor>();
        }
        
        public override void BindView()
        {
            view = OriginView as UIView_Craft;
            
            isDitryDesc = true;
            craftData.OnImteUnlocked += OnItemUnlock;
            if(isFirstBind)
            {
                var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item);
                
                foreach (var itemKey in data.Keys)
                {
                    var itemData = data[itemKey];
                    //含有blueprint的都属于合成物体
                    bool isCraftItem = itemData.ContainsKey(JsonKeys.blueprint);
                    if(isCraftItem)
                        carftItemKeys.Add(itemKey);
                }

                var itemCount = carftItemKeys.Count;
                craftGrids.Capacity = itemCount;
                for (int i = 0; i < itemCount; i++)
                {
                    var grid = uiManager.CreateUIModel(view.CraftGridKey) as UIModel_CraftGrid;
                    grid.GridIndex = i;
                    grid.SetItemKey(carftItemKeys[i]);
                    craftGrids.Add(grid);
                }

                if(craftData.SelectIndex >= itemCount)
                {
                    craftData.SelectIndex = 0;
                }
            }

            {
                var itemCount = carftItemKeys.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    var grid = craftGrids[i];
                    dirtyGridIndices.Add(i);
                    grid.Img_Select = view.Img_Select;
                    grid.SetParent(view.Group_CraftGrid);
                    grid.AddListener(OnButtonDown_CraftGrid);
                }
                view.Btn_Quit.onClick.AddListener(OnButtonDown_Quit);
                view.Btn_Craft.onClick.AddListener(OnButtonDown_Craft);
                view.Btn_Unlock.onClick.AddListener(OnButtonDown_Unlock);
            }
            
            

        }
        public override void UnbindView()
        {
            craftData.OnImteUnlocked -= OnItemUnlock;
            var itemCount = carftItemKeys.Count;
            for (int i = 0; i < itemCount; i++)
            {
                var grid = craftGrids[i];
                grid.Img_Select = null;
                grid.RemoveAllListener();
            }
            view.Btn_Quit.onClick.RemoveAllListeners();
            view.Btn_Craft.onClick.RemoveAllListeners();
            view.Btn_Unlock.onClick.RemoveAllListeners();
            view = null;
        }

        public override void UpdateView(GameTime time)
        {
            if(dirtyGridIndices.Count > 0)
            {
                foreach (var index in dirtyGridIndices)
                {
                    craftGrids[index].UpdateView(time);
                }
                dirtyGridIndices.Clear();
            }
            if(isDitryDesc)
            {
                isDitryDesc = false;
                UpdateDesc();
            }
        }

        public void OnButtonDown_Quit()
        {
            uiManager.CloseWindow(this);
        }

        public void OnButtonDown_CraftGrid(UIModel_CraftGrid craftGrid)
        {
            if(craftData.SelectIndex != craftGrid.GridIndex)
            {
                craftData.SelectIndex = craftGrid.GridIndex;
                isDitryDesc = true;
                dirtyGridIndices.Add(craftGrid.GridIndex);
            }
        }

        public void UpdateDesc()
        {
            var selectIndex = craftData.SelectIndex;
            var key = carftItemKeys[selectIndex];
            var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[key];
            bool isUnlock = craftData.IsUnlock(key);
            var itemName = (string)data[JsonKeys.name];
            var spriteKey = (string)data[JsonKeys.icon];
            view.Txt_Item.text = itemName;
            view.Img_Item.sprite = resPoolManager.LoadResWithKey<Sprite>(spriteKey, ResFlag.Sprite_Icon);
            view.Img_Item.color = isUnlock ? ColorUtil.color_active : ColorUtil.color_inactive;
            view.Img_Unknown.gameObject.SetActive(!isUnlock);
            //刷新词条
            var costsData = data[JsonKeys.cost_craft];
            var kindNum = costsData.Count;
            while (craftCosts.Count != kindNum)
            {
                if(craftCosts.Count > kindNum)
                {
                    var finalIndex = craftCosts.Count - 1;
                    uiManager.ReleaseUIModel(craftCosts[finalIndex]);
                    craftCosts.RemoveAt(finalIndex);
                }
                else
                {
                    var craftCost = uiManager.CreateUIModel(view.CraftCostKey) as UIModel_CraftCost;
                    craftCost.SetParent(view.Group_Desc);
                    craftCosts.Add(craftCost);
                }
            }
            for (int i = 0; i < kindNum; i++)
            {
                craftCosts[i].UpdateView(costsData, i, isUnlock);
            }
            view.Btn_Craft.gameObject.SetActive(isUnlock);
            view.Btn_Unlock.gameObject.SetActive(!isUnlock);
        }

        public int GetItemOutputQuantity(JsonData data)
        {
            int quantity = data.ContainsKey(JsonKeys.outputQuantity) ? (int)data[JsonKeys.outputQuantity] : 1;
            return quantity;
        }

        public void OnButtonDown_Craft()
        {
            var selectIndex = craftData.SelectIndex;
            var key = carftItemKeys[selectIndex];
            var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[key];
            var costsData = data[JsonKeys.cost_craft];
            var costsKindCount = costsData.Count;
            (string, int)[] costs = new (string, int)[costsKindCount];
            bool isEnough = true;
            for (int i = 0; i < costsKindCount; i++)
            {
                var singleData = costsData[i];
                var costKey = (string)singleData[JsonKeys.name];
                var costCount = (int)(int)singleData[JsonKeys.count];
                isEnough &= bagData.GetItemCount(costKey) >= costCount;
                if(!isEnough) break;
                costs[i] = (costKey, costCount);
            }
            if(isEnough)
            {
                for (int i = 0; i < costsKindCount; i++)
                {
                    var cost = costs[i];
                    bagData.LoseItem(cost.Item1, cost.Item2);    
                }
                var quantity = GetItemOutputQuantity(data);
                bagData.ReceiveItem(key, quantity);
                isDitryDesc = true;
            }
        }

        public void OnButtonDown_Unlock()
        {
            var selectIndex = craftData.SelectIndex;
            var key = carftItemKeys[selectIndex];
            if(!craftData.IsUnlock(key))
            {
                var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[key];
                var blueprint = (string)data[JsonKeys.blueprint];
                var isEnough = bagData.GetItemCount(blueprint) > 0;
                if(isEnough)
                {
                    bagData.LoseItem(blueprint, 1);
                    craftData.Unlock(key);
                    isDitryDesc = true;
                }
            }
            
        }
        
        public void OnItemUnlock(string key)
        {
            var index = carftItemKeys.IndexOf(key);
            if(index >= 0) dirtyGridIndices.Add(index);
            isDitryDesc = true;
        }


    }

}
