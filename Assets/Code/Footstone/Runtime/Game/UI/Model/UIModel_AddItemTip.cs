using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIModel_AddItemTip : UIModel
    {
        private UIView_AddItemTip view;
        private string itemKey;
        private int itemCount;
        private bool isItemDirty = false;
        private float showTimer = 0;
        private float moveTimer = 0;
        private bool isMoving = false;
        private float moveStartY = 0;
        private float moveEndY = 0;
        public bool IsShowing { get; private set;}
        public float speedScale = 1.0f;
        

        public UIModel_AddItemTip(UIView view) : base(view)
        {

        }   

        

        public override void UpdateView(GameTime time)
        {
            if(isItemDirty)
            {
                isItemDirty = false;
                var itemData = resPoolManager.LoadItemData(itemKey);
                var iconKey = (string)itemData[JsonKeys.icon];
                var iconSprite = resPoolManager.LoadResWithKey<Sprite>(iconKey, ResFlag.Sprite_Icon);
                view.Img_Item.sprite = iconSprite;

                var itemName = itemData[JsonKeys.name];
                if(itemCount >= 0)
                {
                    view.Text_Item.text = string.Format($"{itemName} {GameConstant.GreenText}", "+" + itemCount);
                }
                else
                {
                    view.Text_Item.text = string.Format($"{itemName} {GameConstant.RedText}", itemCount);
                }
            }

            if(IsShowing)
            {
                showTimer += time.DeltaTime * speedScale;
                float alpha = view.AlphaCurve.Evaluate(showTimer);
                IsShowing = showTimer <= view.AlphaDuration;
                view.Group_Tip.alpha = alpha;
                if(isMoving)
                {
                    moveTimer += time.DeltaTime * speedScale;
                    var progress = view.MoveCurve.Evaluate(moveTimer);
                    var y = Mathf.Lerp(moveStartY, moveEndY, progress);
                    var pos = view.RectTrasn_Tip.localPosition;
                    pos.y = y;
                    view.RectTrasn_Tip.localPosition = pos;
                    isMoving = moveTimer <= view.MoveDuration;
                }
            }
            
        }

        public void StartShow(string key, int count)
        {
            itemKey = key;
            itemCount = count;
            isItemDirty = true;
            showTimer = 0;
            IsShowing = true;
        }

        public void StartMove(float endY)
        {
            moveStartY = view.RectTrasn_Tip.localPosition.y;
            moveEndY = endY;
            moveTimer = 0;
            isMoving = true;
        }


        public override void BindView()
        {
            view = OriginView as UIView_AddItemTip;
            
        }

        public override void UnbindView()
        {
            IsShowing = false;
            isMoving = false;
        }
    }

}
