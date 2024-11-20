using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{

    public class UIView_AddItemTip : UIView
    {
        public CanvasGroup Group_Tip;
        public RectTransform RectTrasn_Tip;
        public Image Img_Item;
        public TextMeshProUGUI Text_Item;

        public AnimationCurve AlphaCurve;
        public AnimationCurve MoveCurve;

        public float AlphaDuration { get => AlphaCurve.keys[AlphaCurve.length - 1].time; }
        public float MoveDuration { get => MoveCurve.keys[MoveCurve.length - 1].time; }

        protected override UIModel CreateModel()
        {
            return new UIModel_AddItemTip(this);
        }
    }

}
