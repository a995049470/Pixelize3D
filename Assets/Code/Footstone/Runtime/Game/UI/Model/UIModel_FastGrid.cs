using Lost.Runtime.Footstone.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lost.Runtime.Footstone.Game
{

    public class UIModel_FastGrid : UIModel
    {
        private UIView_FastGrid view;
        public int GridIndex;

        public UIModel_FastGrid(UIView view) : base(view)
        {
        }

        public override void BindView()
        {
            view = OriginView as UIView_FastGrid;
            
        }

        public override void UpdateView(GameTime time)
        {
            bool isClear = true;
            if(GridIndex >= 0)
            {
                var gridData = bagData.GetBagGridItemData(GridIndex);
                if(gridData.IsVaild())
                {
                    isClear = false;
                    var maxNum = gridData.GetMaxNum();
                    if(maxNum > 1) view.Text_Num.text = gridData.ItemCount.ToString();
                    else view.Text_Num.text = "";

                    var sprite = resPoolManager.LoadResWithKey<Sprite>(gridData.GetIconKey(), ResFlag.Sprite_Icon);
                    view.Img_Icon.enabled = true;
                    view.Img_Icon.sprite = sprite;
                }
            }
            if(isClear)
            {
                view.Text_Num.text = "";
                view.Img_Icon.enabled = false;
            }

            var isFastSelected = bagData.FastSelectIndex == GridIndex;
            var key = isFastSelected ? view.SpriteKey_Select : view.SpriteKey_Slot;
            view.Img_Bg.sprite = resPoolManager.LoadResWithKey<Sprite>(key, ResFlag.Sprite_UI);
        }
        

        public void AddListener(EventTriggerType triggerType, FastGridEventCallback listener)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = triggerType;
            entry.callback.AddListener((data) => listener?.Invoke(data, this));
            view.Trigger_Grid.triggers.Add(entry);
        }

        public void RemoveAllListener()
        {
            view.Trigger_Grid.triggers.Clear();
        }
        

        public override void UnbindView()
        {
            
        }
    }

}
