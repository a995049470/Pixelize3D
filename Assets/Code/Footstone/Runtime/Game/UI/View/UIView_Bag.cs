using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Lost.Runtime.Footstone.Game
{

    public class UIView_Bag : UIView
    {
        public Transform Group_Bag;
        public Button Btn_Quit;
        public Image Img_Drag;
        public string ModelKey_Grid;
        public string ModelKey_Tip;
        

        protected override UIModel CreateModel()
        {
            return new UIWindow_Bag(this);
        }
    }

}
