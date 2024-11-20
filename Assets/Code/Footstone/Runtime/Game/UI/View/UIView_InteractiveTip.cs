using TMPro;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_InteractiveTip : UIView
    {
        public TextMeshProUGUI Txt_Tip;
        public RectTransform Rect_Select;
        public TextMeshProUGUI[] TxtArray_Option;

        protected override UIModel CreateModel()
        {
            return new UIWindow_InteractiveTip(this);
        }
    }

}
