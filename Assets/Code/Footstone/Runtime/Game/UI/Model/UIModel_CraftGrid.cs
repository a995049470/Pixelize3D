using LitJson;
using Lost.Runtime.Footstone.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{

    public class UIModel_CraftGrid : UIModel
    {
        private string itemKey;
        private JsonData itemData;
        public Image Img_Select;
        private UIView_CraftGrid view;
        public int GridIndex;
        private CraftProcessor craftProcessor;
        private CraftData craftData { get => craftProcessor.SingleComponent.Data; }
        

        public UIModel_CraftGrid(UIView view) : base(view)
        {
            
        }

        protected override void InitializeProcessors()
        {
            base.InitializeProcessors();
            craftProcessor = GetProcessor<CraftProcessor>();
        }

        public override void BindView()
        {
            view = OriginView as UIView_CraftGrid;
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
                itemData = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[key];
            }   
        }

        public void AddListener(CarftGridClickEvent clickEvent)
        {
            view.Btn_Grid.onClick.AddListener(() => clickEvent.Invoke(this));
        }

        public void RemoveAllListener()
        {
            view.Btn_Grid.onClick.RemoveAllListeners();
        }
        
        


        public override void UpdateView(GameTime time)
        {
            bool isUnlock = craftData.IsUnlock(itemKey);
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
                var key_blueprint = (string)itemData[JsonKeys.blueprint];
                bool isCanUnlock = bagData.GetItemCount(key_blueprint) > 0;
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

            if(GridIndex == craftData.SelectIndex)
            {
                Img_Select.transform.SetParent(view.transform);
                Img_Select.transform.localPosition = Vector3.zero;
            }
        }
    }

}
