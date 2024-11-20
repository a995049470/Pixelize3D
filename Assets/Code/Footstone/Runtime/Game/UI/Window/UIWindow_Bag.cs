using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lost.Runtime.Footstone.Game
{

    public class UIWindow_Bag : UIWindow
    {
        private UIView_Bag view;
        private HashSet<int> dirtyGridIdSet;
        private UIModel_BagGrid[] grids;
        private UIModel_Tip tip;
        private int currentTipGridIndex;
        private int targetTipGridIndex;
        private int selectDragIndex = -1;
        private int targetDragIndex = -1;
        private bool isDirtySelectDragIndex = false;
        private InputManager inputManager;
        private bool isDitryDragSprite = true;
        

        public override int Orderer => GameConstant.Bag;

        public UIWindow_Bag(UIView view) : base(view)
        {
            dirtyGridIdSet = new HashSet<int>();
        }

        protected override void InitializeService(IServiceRegistry service)
        {
            inputManager = service.GetService<InputManager>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_Bag;
            currentTipGridIndex = -1;
            targetTipGridIndex = -1;
            tip = tip ?? (uiManager.CreateUIModel(view.ModelKey_Tip) as UIModel_Tip);
            tip.SetActive(false);
            tip.SetParent(view.transform);
            var gridNum = bagData.GridNum;
            var childCount = view.Group_Bag.childCount;
            grids = grids ?? new UIModel_BagGrid[gridNum];
            for (int i = 0; i < gridNum; i++)
            {
                if(i >= childCount)
                {
                    var model_grid = uiManager.CreateUIModel(view.ModelKey_Grid) as UIModel_BagGrid;
                    model_grid.GridIndex = i;
                    grids[i] = model_grid;
                    model_grid.SetParent(view.Group_Bag);
                }

                {
                    var model_grid = grids[i];
                    model_grid.AddListener(EventTriggerType.PointerDown, OnPointDown_BagGrid);
                    model_grid.AddListener(EventTriggerType.PointerEnter, OnPointEnter_BagGrid);
                    model_grid.AddListener(EventTriggerType.PointerExit, OnPointExit_BagGrid);
                }

                dirtyGridIdSet.Add(i);
            }
            bagData.OnGridItemDataChanged += OnGridItemDataChange;
            view.Btn_Quit.onClick.AddListener(OnClick_Quit);
        }
        
        public override void UpdateView(GameTime time)
        {
            //tip
            {
                bool isUpdateTip = false;
                if (currentTipGridIndex != targetTipGridIndex) isUpdateTip = true;
                else if (dirtyGridIdSet.Contains(targetTipGridIndex)) isUpdateTip = true;

                if (isUpdateTip)
                {
                    currentTipGridIndex = targetTipGridIndex;
                    tip.TargetGridPos = currentTipGridIndex;
                    tip.UpdateView(time);
                    if(currentTipGridIndex >= 0)
                        tip.OriginView.transform.position = grids[currentTipGridIndex].GetTipPosition();
                }
            }
            if(dirtyGridIdSet.Count > 0)
            {

                foreach (var id in dirtyGridIdSet)
                {
                    grids[id].UpdateView(time);
                }
                dirtyGridIdSet.Clear();
            }

            if(isDirtySelectDragIndex)
            {
                isDirtySelectDragIndex = false;
                if(selectDragIndex >= 0)
                {
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

                view.Img_Drag.rectTransform.anchoredPosition = inputManager.GetUIMousePoistion();

                if(inputManager.IsLeftMouseButtonUp())
                {
                    if(targetDragIndex >= 0 && selectDragIndex != targetDragIndex)
                    {
                        bagData.TrySwitchItem(selectDragIndex, targetDragIndex);
                    }
                    targetTipGridIndex = targetDragIndex;
                    selectDragIndex = -1;
                    isDirtySelectDragIndex = true;
                    targetDragIndex = -1;
                }

            
            }
            
        }

        public override void UnbindView()
        {
            bagData.OnGridItemDataChanged -= OnGridItemDataChange;
            view.Btn_Quit.onClick.RemoveAllListeners();
            foreach (var grid in grids)
            {
                grid.RemoveAllListener();
            }
            targetTipGridIndex = -1;
            selectDragIndex = -1;
            isDirtySelectDragIndex = true;
            targetDragIndex = -1;
        }

        private void OnGridItemDataChange(int id)
        {
            dirtyGridIdSet.Add(id);
        }

        private void OnClick_Quit()
        {
            uiManager.CloseWindow(this);
        }

        private void OnPointEnter_BagGrid(BaseEventData eventData, UIModel_BagGrid grid)
        {   
            if(selectDragIndex >= 0)
            {
                targetDragIndex = grid.GridIndex;
                isDitryDragSprite = true;
            }
            else
            {
                targetTipGridIndex = grid.GridIndex;
            }

        }

        private void OnPointExit_BagGrid(BaseEventData eventData, UIModel_BagGrid grid)
        {
            if(selectDragIndex >= 0)
            {
                if(targetDragIndex == grid.GridIndex) targetDragIndex = -1;
                isDitryDragSprite = true;
            }
            else
            {
                if(targetTipGridIndex == grid.GridIndex) targetTipGridIndex = -1;
            }
        }

        private void OnPointDown_BagGrid(BaseEventData eventData, UIModel_BagGrid grid)
        {
            var isLeftButtonDown = eventData.currentInputModule.input.GetMouseButtonDown(0);
            if(isLeftButtonDown)
            {
                var itemData = bagData.GetBagGridItemData(grid.GridIndex);
                if(itemData.IsVaild())
                {
                    selectDragIndex = grid.GridIndex;
                    targetDragIndex = grid.GridIndex;
                    isDirtySelectDragIndex = true;
                    isDitryDragSprite = true;
                }
                targetTipGridIndex = -1;
            }
        }

        


    }

}
