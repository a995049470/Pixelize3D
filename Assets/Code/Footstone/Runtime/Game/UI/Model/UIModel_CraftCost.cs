using LitJson;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIModel_CraftCost : UIModel
    {
        private UIView_CraftCost view;
        private string unknownKey { get => GameConstant.IconKye_Unknown; }
        private string unknownName { get => GameConstant.Name_Unknown; }
        private string unknownCount { get => GameConstant.Num_Unknown; }
        public UIModel_CraftCost(UIView view) : base(view)
        {
            
        }

        public void UpdateView(JsonData costsData, int index, bool isUnlock)
        {
            if(isUnlock)
            {
                var costData = costsData[index];
                var key = (string)costData[JsonKeys.name];
                var targetCount = (int)costData[JsonKeys.count];
                var currentCount = bagData.GetItemCount(key);
                var itemData = resPoolManager.LoadResConfigData(ResFlag.Config_Item)[key];
                var itemName = (string)itemData[JsonKeys.name];
                var spriteKey = (string)itemData[JsonKeys.icon];
                view.Img_Cost.sprite = resPoolManager.LoadResWithKey<Sprite>(spriteKey, ResFlag.Sprite_Icon);
                view.Txt_Cost.text = itemName;
                view.Txt_Num.text = $"{currentCount}/{targetCount}";
            }
            else
            {
                view.Img_Cost.sprite = resPoolManager.LoadResWithKey<Sprite>(unknownKey, ResFlag.Sprite_Icon);
                view.Txt_Cost.text = unknownName;
                view.Txt_Num.text = unknownCount;
            }
        }

        public override void BindView()
        {
            view = OriginView as UIView_CraftCost;
        }

        public override void UnbindView()
        {
            view = null;
        }
    }

}
