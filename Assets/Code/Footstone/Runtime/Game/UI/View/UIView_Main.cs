using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEngine.UI;
namespace Lost.Runtime.Footstone.Game
{

    public class UIView_Main : UIView
    {
        public Button Btn_Bag;
        public Button Btn_Craft;
        public Button Btn_Map;
        public RectTransform Group_Fast;
        public string Key_FastGrid;
        public string Key_Bag = "Model_Bag";
        public string Key_Craft = "Model_Craft";
        public string Key_Map = "Window_Map";
        public Image Img_Hp;
        public Image Img_Energy;
        public RectTransform Area_ItemTip;
        public string Key_ItemTip = "Model_ItemTip";
        public int ItemTipWidth = 100;
        public int TipMaxCount = 5;
        public Image Img_Drag;
        public RectTransform Trans_InteractiveTip;
        public float UpspeedPerExtraTip = 0.0f;
        protected override UIModel CreateModel()
        {
            return new UIWindow_Main(this);
        }
    }

}
