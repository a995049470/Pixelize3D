using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{

    public class UIView_Craft : UIView
    {
        public RectTransform Group_CraftGrid;
        public RectTransform Group_Desc;
        public Image Img_Select;
        public Image Img_Item;
        public Image Img_Unknown;
        public Button Btn_Quit;
        public Button Btn_Unlock;
        public Button Btn_Craft;
        public TextMeshProUGUI Txt_Item;
        public string CraftGridKey;
        public string CraftCostKey;
        protected override UIModel CreateModel()
        {
            return new UIWindow_Craft(this);
        }
    }

}
