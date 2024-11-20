using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lost.Runtime.Footstone.Game
{

    public class UIWindow_Main : UIWindow
    {
        private UIView_Main view;
        private UIModel_FastGrid[] fastGrids;
        private HashSet<int> dirtyGridIndices;
        private Queue<(string, int)> addItems;
        private List<UIModel_AddItemTip> addItemTips;
        private PlayerHitPointProcessor playerHitPointProcessor;
        private PlayerEnergeProcessor playerEnergeProcessor;
        private CameraProcessor cameraProcessor;
        private TipProcessor tipProcessor;
        private InputManager inputManager;
        private int selectDragIndex = -1;
        private int targetDragIndex = -1;
        private bool isDirtySelectDragIndex = true;
        private bool isDitryDragSprite = true;
        private float dragTimpr = 0;
        private const float dragDelayTime = 0.15f;

        public override int Orderer => GameConstant.Main;
        public override bool AllowGameInput => true;

        public UIWindow_Main(UIView view) : base(view)
        {
            dirtyGridIndices = new HashSet<int>();
            addItems = new();
            addItemTips = new();
        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            playerHitPointProcessor = GetProcessor<PlayerHitPointProcessor>();
            playerEnergeProcessor = GetProcessor<PlayerEnergeProcessor>();
            cameraProcessor = GetProcessor<CameraProcessor>();
            tipProcessor = GetProcessor<TipProcessor>();
        }

        protected override void InitializeService(IServiceRegistry service)
        {
            inputManager = service.GetService<InputManager>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_Main;
            view.Btn_Bag.onClick.AddListener(OnClick_Bag);
            view.Btn_Craft.onClick.AddListener(OnClick_Craft);
            view.Btn_Map.onClick.AddListener(OnClick_Map);
            var gridNum = bagData.Width;
            if(isFirstBind)
            {
                fastGrids = fastGrids ?? new UIModel_FastGrid[gridNum];
                for (int i = 0; i < gridNum; i++)
                {

                    var model_grid = uiManager.CreateUIModel(view.Key_FastGrid) as UIModel_FastGrid;
                    model_grid.GridIndex = i;
                    model_grid.SetParent(view.Group_Fast);
                    fastGrids[i] = model_grid;
                }
            }
            for (int i = 0; i < gridNum; i++)
            {
                var model_grid = fastGrids[i];
                model_grid.AddListener(EventTriggerType.PointerClick, OnClick_FastGrid);
                model_grid.AddListener(EventTriggerType.PointerEnter, OnPointEnter_FastGrid);
                model_grid.AddListener(EventTriggerType.PointerExit, OnPointExit_FastGrid);
                model_grid.AddListener(EventTriggerType.PointerDown, OnPointDown_FastGrid);
                dirtyGridIndices.Add(i);
            }            
            bagData.OnItemCountChange += OnItemCountChanage;
        }

        public override void UpdateView(GameTime time)
        {
            if(dirtyGridIndices.Count > 0)
            {
                foreach (var index in dirtyGridIndices)
                {
                    if(index >= 0 && index < fastGrids.Length)
                    {
                        fastGrids[index].UpdateView(time);
                    }
                }
            }

            var hitPointComp = playerHitPointProcessor.HitPointComp;
            var energyComp = playerEnergeProcessor.EnergyComp;
            if(hitPointComp != null)
                view.Img_Hp.fillAmount = hitPointComp.HPPercent;
            if(energyComp != null)
                view.Img_Energy.fillAmount = energyComp.EnergyPercent;  

            if(addItems.Count + addItemTips.Count > 0)
            {
                var newAddCount = Mathf.Min(view.TipMaxCount - addItemTips.Count, addItems.Count);
                if(newAddCount > 0)
                {
                    for (int i = 0; i < addItemTips.Count; i++)
                    {
                        var itemTip = addItemTips[i];
                        var y = (i + newAddCount) * view.ItemTipWidth;
                        itemTip.StartMove(y);
                    }
                    for (int i = 0; i < newAddCount; i++)
                    {
                        var item = addItems.Dequeue();
                        var model = uiManager.CreateUIModel(view.Key_ItemTip) as UIModel_AddItemTip;
                        model.SetParent(view.Area_ItemTip);
                        var pos = new Vector2(0, view.ItemTipWidth * (newAddCount - 1 - i));
                        model.OriginView.RectTransform.localPosition = pos;
                        model.StartShow(item.Item1, item.Item2);
                        addItemTips.Insert(0, model);
                    }
                    
                }
                
                float extraCount = addItems.Count + addItemTips.Count - view.TipMaxCount;
                float speedScale = Mathf.Max(0, extraCount) * Mathf.Max(0, view.UpspeedPerExtraTip) + 1.0f;
                for (int i = addItemTips.Count - 1; i >= 0 ; i--)
                {
                
                    var itemTip = addItemTips[i];
                    itemTip.speedScale = speedScale;
                    itemTip.UpdateView(time);
                    if(!itemTip.IsShowing)
                    {
                        addItemTips.RemoveAt(i);
                        uiManager.ReleaseUIModel(itemTip);
                    }
                }
            }

            if(isDirtySelectDragIndex)
            {
                isDirtySelectDragIndex = false;
                if(selectDragIndex >= 0)
                {
                    var itemData = bagData.GetBagGridItemData(selectDragIndex);
                    view.Img_Drag.gameObject.SetActive(true);
                }
                else
                {
                    view.Img_Drag.gameObject.SetActive(false);
                }
            }

            if(selectDragIndex >= 0)
            {
                if(isDitryDragSprite)
                {   
                    isDitryDragSprite = false;
                    var itemData = bagData.GetBagGridItemData(selectDragIndex);
                    var key = targetDragIndex >= 0 ?  itemData.GetIconKey() : GameConstant.IconKey_Ban;
                    view.Img_Drag.sprite = resPoolManager.LoadResWithKey<Sprite>(key, ResFlag.Sprite_Icon);
                }

                if(dragTimpr < dragDelayTime)
                {
                    dragTimpr += time.DeltaTime;
                    isDirtySelectDragIndex |= dragTimpr>= dragDelayTime;
                }
                view.Img_Drag.rectTransform.anchoredPosition = inputManager.GetUIMousePoistion();
                if(inputManager.IsLeftMouseButtonUp())
                {
                    if(targetDragIndex >= 0 && selectDragIndex != targetDragIndex)
                    {
                        bagData.TrySwitchItem(selectDragIndex, targetDragIndex);
                    }
                    selectDragIndex = -1;
                    isDirtySelectDragIndex = true;
                    targetDragIndex = -1;
                }
            }
            
            //更新交互提示
            {
                var interactiveTipInfo = tipProcessor.SingleComponent.InteractiveTipInfo;
                if(interactiveTipInfo.IsVaildTip())
                {
                    view.Trans_InteractiveTip.gameObject.SetActive(true);
                    var parent = view.Trans_InteractiveTip.parent as RectTransform;
                    var position = interactiveTipInfo.Position;
                    var camera = cameraProcessor.SingleComponent.Component;
                    var screenPosition = camera.WorldToScreenPoint(position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPosition, null, out var localPoint);
                    view.Trans_InteractiveTip.anchoredPosition = localPoint;
                }
                else
                {
                    view.Trans_InteractiveTip.gameObject.SetActive(false);
                }
            }
                     
        }

        public void OnClick_FastGrid(BaseEventData eventData, UIModel_FastGrid fastGrid)
        {
            
            if(fastGrid.GridIndex == targetDragIndex || selectDragIndex < 0)
            {
                bagData.SetFastSelectIndex(fastGrid.GridIndex);
                
            }
        }

        private void OnPointEnter_FastGrid(BaseEventData eventData, UIModel_FastGrid grid)
        {   
            if(selectDragIndex >= 0)
            {
                targetDragIndex = grid.GridIndex;
                isDitryDragSprite = true;
            }
        }

        private void OnPointExit_FastGrid(BaseEventData eventData, UIModel_FastGrid grid)
        {
            if(selectDragIndex >= 0)
            {
                if(targetDragIndex == grid.GridIndex) targetDragIndex = -1;
                isDitryDragSprite = true;
            }
        }

        private void OnPointDown_FastGrid(BaseEventData eventData, UIModel_FastGrid grid)
        {
            var itemData = bagData.GetBagGridItemData(grid.GridIndex);
            if(itemData.IsVaild())
            {
                selectDragIndex = grid.GridIndex;
                targetDragIndex = grid.GridIndex;
                isDitryDragSprite = true;
                dragTimpr = 0;
            }
        }

        public void OnGridItemDataChange(int index)
        {
            dirtyGridIndices.Add(index);
        }

        public void OnItemCountChanage(string itemKey, int itemCount)
        {
            addItems.Enqueue((itemKey, itemCount));
        }

        public override void UnbindView()
        {
            view.Btn_Bag.onClick.RemoveAllListeners();
            view.Btn_Craft.onClick.RemoveAllListeners();
            view.Btn_Map.onClick.RemoveAllListeners();
            foreach (var fastGrid in fastGrids)
            {
                fastGrid.RemoveAllListener();
            }
            bagData.OnItemCountChange -= OnItemCountChanage; 
        }

        private void OnClick_Bag()
        {
            uiManager.OpenWindow(view.Key_Bag);
        }

        private void OnClick_Craft()
        {
            uiManager.OpenWindow(view.Key_Craft);
        }

        private void OnClick_Map()
        {
            uiManager.OpenWindow(view.Key_Map);
        }


        public override void OnTop()
        {
            base.OnTop();
            SetFastGridActive(true);
        }

        public override void OnCovered()
        {
            base.OnCovered();
            SetFastGridActive(false);
        }

        private void SetFastGridActive(bool active)
        {
            view.Group_Fast.parent.gameObject.SetActive(active);
        }
    }

}
