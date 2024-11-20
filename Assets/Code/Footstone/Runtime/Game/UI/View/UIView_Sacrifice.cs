using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_Sacrifice : UIView
    {
        public RectTransform Group_OblationGrid;
        public Image Img_Select;
        public Image Img_Item;
        public Image Img_Unknown;
        public TextMeshProUGUI Txt_Item;

        public Image Img_BaseRewards;
        public TextMeshProUGUI Txt_BaseRewards;

        public Image Img_StageRewards;
        public TextMeshProUGUI Txt_StageRewards;

        public Image Img_Progress;
        public TextMeshProUGUI Txt_Progress;

        public Button Btn_Small;
        public Button Btn_Big;
        public Button Btn_First;

        public Button Btn_Quit;

        public string Key_OblationGrid;

        protected override UIModel CreateModel()
        {
            return new UIWindow_Sacrifice(this);
        }
    }

}
