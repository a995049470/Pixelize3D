using TMPro;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_CraftCost : UIView
    {
        public Image Img_Cost;
        public TextMeshProUGUI Txt_Cost;
        public TextMeshProUGUI Txt_Num;

        protected override UIModel CreateModel()
        {
            return new UIModel_CraftCost(this);
        }
    }

}
