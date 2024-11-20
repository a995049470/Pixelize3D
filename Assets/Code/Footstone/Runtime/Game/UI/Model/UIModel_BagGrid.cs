using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lost.Runtime.Footstone.Game
{

    public class UIModel_BagGrid : UIModel
    {
        private UIView_BagGrid view;
        public int GridIndex;

        public UIModel_BagGrid(UIView view) : base(view)
        {
            
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
        }
        
        public Vector3 GetTipPosition()
        {
            return view.Img_Icon.transform.position;
        }

        public void AddListener(EventTriggerType triggerType, BagGridEventCallback listener)
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
        
        public void SetGridPos(int pos)
        {
            GridIndex = pos;
        }
        
        public override void BindView()
        {
            view = OriginView as UIView_BagGrid;
            GridIndex = -1;
        }

        public override void UnbindView()
        {
        }
    }

}
