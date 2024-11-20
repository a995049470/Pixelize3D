using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIWindow_Sacrifice : UIWindow
    {
        private UIView_Sacrifice view;
        private List<string> sacrificeItemKeys = new();
        private List<UIModel_OblationGrid> oblationGrids = new();
        private HashSet<int> dirtyGridIndices = new();
        private bool isDitryDesc;
        private string infinity = "∞";
        private string unknownKey = "unknown";
        private string unknown = "?";

        private SacrificeProcessor sacrificeProcessor;
        private SacrificeComponent sacrificeComponent { get => sacrificeProcessor.SingleComponent; }
        public override int Orderer => GameConstant.Sacrifice;

        public UIWindow_Sacrifice(UIView view) : base(view)
        {
        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            sacrificeProcessor = GetProcessor<SacrificeProcessor>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_Sacrifice;
            if(isFirstBind)
            {
                var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item);
                
                foreach (var itemKey in data.Keys)
                {
                    var itemData = data[itemKey];
                    bool isOblationItem = itemData.ContainsKey(JsonKeys.rewards_base) && itemData.ContainsKey(JsonKeys.rewards_stage);
                    if(isOblationItem)
                        sacrificeItemKeys.Add(itemKey);
                }

                var itemCount = sacrificeItemKeys.Count;
                oblationGrids.Capacity = itemCount;
                for (int i = 0; i < itemCount; i++)
                {
                    var grid = uiManager.CreateUIModel(view.Key_OblationGrid) as UIModel_OblationGrid;
                    grid.GridIndex = i;
                    grid.SetItemKey(sacrificeItemKeys[i]);
                    oblationGrids.Add(grid);
                }

                if(sacrificeComponent.SelectIndex >= itemCount)
                {
                    sacrificeComponent.SelectIndex = 0;
                }
            }

            {
                isDitryDesc = true;
                var itemCount = sacrificeItemKeys.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    var grid = oblationGrids[i];
                    dirtyGridIndices.Add(i);
                    grid.Img_Select = view.Img_Select;
                    grid.SetParent(view.Group_OblationGrid);
                    grid.AddListener(OnButtonDown_OblationGrid);
                }
                view.Btn_First.onClick.AddListener(OnButtonDown_First);
                view.Btn_Small.onClick.AddListener(OnButtonDown_Small);
                view.Btn_Big.onClick.AddListener(OnButtonDown_Big);
                view.Btn_Quit.onClick.AddListener(OnButtonDown_Quit);
            }
        }

        public override void UnbindView()
        {
            var itemCount = sacrificeItemKeys.Count;
            for (int i = 0; i < itemCount; i++)
            {
                var grid = oblationGrids[i];
                grid.Img_Select = null;
                grid.RemoveAllListener();
            }
            view.Btn_First.onClick.RemoveAllListeners();
            view.Btn_Small.onClick.RemoveAllListeners();
            view.Btn_Big.onClick.RemoveAllListeners();
            view.Btn_Quit.onClick.RemoveAllListeners();
            view = null;
        }

        private void UpdateDesc()
        {
            var selectIndex = sacrificeComponent.SelectIndex;
            var itemKey = sacrificeItemKeys[selectIndex];
            var oblationData = sacrificeComponent.GetOblationData(itemKey);
            var itemData = oblationData.ItemData;
            bool isUnlock = oblationData.TotalExp > 0;
            var itemName = (string)itemData[JsonKeys.name];
            var spriteKey = (string)itemData[JsonKeys.icon];
            view.Txt_Item.text = itemName;
            view.Img_Item.sprite = resPoolManager.LoadResWithKey<Sprite>(spriteKey, ResFlag.Sprite_Icon);
            view.Img_Item.color = isUnlock ? ColorUtil.color_active : ColorUtil.color_inactive;
            view.Img_Unknown.gameObject.SetActive(!isUnlock);
            string icon_base;
            string icon_stage;
            string num_base;
            string num_stage;
            string exp_next;
            float progress;

            if (isUnlock)
            {
                var reward_base = oblationData.GetBaseRewards();
                icon_base = resPoolManager.LoadItemData(reward_base.Item1)[JsonKeys.icon].ToString();
                num_base = reward_base.Item2.ToString();
                if(oblationData.TryGetStageExp(out var exp_stage))
                {
                    progress = (float)oblationData.TotalExp / exp_stage;
                    exp_next = exp_stage.ToString();
                }
                else
                {
                    progress = 1;
                    exp_next = infinity;
                }

                if(oblationData.TryGetStageRewards(out var rewards_stage))
                {
                    icon_stage = resPoolManager.LoadItemData(rewards_stage.Item1)[JsonKeys.icon].ToString();
                    num_stage = reward_base.Item2.ToString();
                }
                else
                {
                    icon_stage = unknownKey;
                    num_stage = unknown;
                }
            }
            else
            {
                progress = 0;
                icon_base = unknownKey;
                num_base = unknown;
                icon_stage = unknownKey;
                num_stage = unknown;
                if(oblationData.TryGetStageExp(out var exp_stage))
                {
                    exp_next = exp_stage.ToString();
                }
                else
                {
                    exp_next = infinity;
                }
            }

            view.Img_BaseRewards.sprite = resPoolManager.LoadResWithKey<Sprite>(icon_base, ResFlag.Sprite_Icon);
            view.Img_StageRewards.sprite = resPoolManager.LoadResWithKey<Sprite>(icon_stage, ResFlag.Sprite_Icon);
            view.Txt_BaseRewards.text = num_base;
            view.Txt_StageRewards.text = num_stage;
            view.Img_Progress.fillAmount = progress;
            view.Txt_Progress.text = $"{oblationData.TotalExp} / {exp_next}";
            view.Btn_First.gameObject.SetActive(!isUnlock);
            view.Btn_Small.gameObject.SetActive(isUnlock);
            view.Btn_Big.gameObject.SetActive(isUnlock);
        }

        public override void UpdateView(GameTime time)
        {
            base.UpdateView(time);
            if(dirtyGridIndices.Count > 0)
            {
                foreach (var index in dirtyGridIndices)
                {
                    oblationGrids[index].UpdateView(time);
                }
                dirtyGridIndices.Clear();
            }

            if(isDitryDesc)
            {
                isDitryDesc = false;
                UpdateDesc();
            }
        }

        private void OnButtonDown_First()
        {
            var itemKey = sacrificeItemKeys[sacrificeComponent.SelectIndex];
            if(sacrificeComponent.TrySacrifice(itemKey, 1, 1))
            {
                isDitryDesc = true;
                dirtyGridIndices.Add(sacrificeComponent.SelectIndex);
            }
        }
        
        private void OnButtonDown_Small()
        {
            var itemKey = sacrificeItemKeys[sacrificeComponent.SelectIndex];
            if(sacrificeComponent.TrySacrifice(itemKey, 1, 1))
            {
                isDitryDesc = true;
            }
        }

        private void OnButtonDown_Big()
        {
            var itemKey = sacrificeItemKeys[sacrificeComponent.SelectIndex];
            if(sacrificeComponent.TrySacrifice(itemKey, 10, 10))
            {
                isDitryDesc = true;
            }
        }

        private void OnButtonDown_Quit()
        {
            uiManager.CloseWindow(this);
        }

        private void OnButtonDown_OblationGrid(UIModel_OblationGrid oblationGrid)
        {
            if(sacrificeComponent.SelectIndex != oblationGrid.GridIndex)
            {
                sacrificeComponent.SelectIndex = oblationGrid.GridIndex;
                isDitryDesc = true;
                dirtyGridIndices.Add(oblationGrid.GridIndex);
            }
        }


        
    }

}
