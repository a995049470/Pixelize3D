using Lost.Runtime.Footstone.Game;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_BagGrid : UIView
    {
        public Image Img_Icon;
        public TextMeshProUGUI Text_Num;
        public EventTrigger Trigger_Grid;
        
        protected override UIModel CreateModel()
        {
            return new UIModel_BagGrid(this);
        }
    }

}
