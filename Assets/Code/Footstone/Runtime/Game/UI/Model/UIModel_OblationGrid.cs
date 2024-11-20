using System;
using LitJson;
using Lost.Runtime.Footstone.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIModel_OblationGrid : UIModel
    {
        private UIView_OblationGrid view;
        private string itemKey;
        private OblationData oblationData;
        public Image Img_Select;
        public int GridIndex;
        
        private SacrificeProcessor sacrificeProcessor;

        public UIModel_OblationGrid(UIView view) : base(view)
        {
        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            sacrificeProcessor = GetProcessor<SacrificeProcessor>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_OblationGrid;
        }

        public override void UnbindView()
        {
            view = null;
        }

        public void SetItemKey(string key)
        {
            if(key != itemKey)
            {
                itemKey = key;
                oblationData = sacrificeProcessor.SingleComponent.GetOblationData(key);
            }   
        }

        public void AddListener(OblationGridClickEvent clickEvent)
        {
            view.Btn_Grid.onClick.AddListener(() => clickEvent.Invoke(this));
        }

        public void RemoveAllListener()
        {
            view.Btn_Grid.onClick.RemoveAllListeners();
        }

        public override void UpdateView(GameTime time)
        {
            var itemData = oblationData.ItemData;
            bool isUnlock = oblationData.TotalExp > 0;
            var spriteKey = (string)itemData[JsonKeys.icon];
            if(isUnlock)
            {   
                view.Img_Exclamation.gameObject.SetActive(false);
                view.Img_Question.gameObject.SetActive(false);
                view.Img_Item.sprite = resPoolManager.LoadResWithKey<Sprite>(spriteKey, ResFlag.Sprite_Icon);
                view.Img_Item.color = ColorUtil.color_active;

            }
            else
            {
                bool isCanUnlock = bagData.GetItemCount(itemKey) > 0;
                if(isCanUnlock)
                {
                    view.Img_Question.gameObject.SetActive(false);
                    view.Img_Exclamation.gameObject.SetActive(true);
                }
                else
                {
                    view.Img_Question.gameObject.SetActive(true);
                    view.Img_Exclamation.gameObject.SetActive(false);
                }
                view.Img_Item.sprite = resPoolManager.LoadResWithKey<Sprite>(spriteKey, ResFlag.Sprite_Icon);
                view.Img_Item.color = ColorUtil.color_inactive;
            }

            if(GridIndex == sacrificeProcessor.SingleComponent.SelectIndex)
            {
                Img_Select.transform.SetParent(view.transform);
                Img_Select.transform.localPosition = Vector3.zero;
            }
        }
    }

}
