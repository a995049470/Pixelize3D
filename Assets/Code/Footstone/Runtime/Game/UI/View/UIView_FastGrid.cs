using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    //快捷栏格子
    public class UIView_FastGrid : UIView
    {
        public Image Img_Bg;
        public Image Img_Icon;
        public TextMeshProUGUI Text_Num;
        public EventTrigger Trigger_Grid;
        public string SpriteKey_Select;
        public string SpriteKey_Slot;
        
        protected override UIModel CreateModel()
        {
            return new UIModel_FastGrid(this);
        }
    }

}
