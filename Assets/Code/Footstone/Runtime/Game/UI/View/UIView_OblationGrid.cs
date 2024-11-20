using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_OblationGrid : UIView
    {
        public Button Btn_Grid;
        public Image Img_Question;
        public Image Img_Exclamation;
        public Image Img_Item;
        protected override UIModel CreateModel()
        {
            return new UIModel_OblationGrid(this);
        }
    }

}
