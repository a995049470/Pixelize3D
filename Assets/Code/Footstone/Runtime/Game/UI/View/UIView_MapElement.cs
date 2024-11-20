using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{
    public class UIView_MapElement : UIView
    {
        public Image Img_Element;
        protected override UIModel CreateModel()
        {
            return new UIModel_MapElement(this);
        }
    }

}
