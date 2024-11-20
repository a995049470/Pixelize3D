using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class UIModel_MapElement : UIModel
    {
        private UIView_MapElement view;

        public UIModel_MapElement(UIView view) : base(view)
        {
        }

        public override void BindView()
        {
            view = OriginView as UIView_MapElement;
        }

        public override void UnbindView()
        {
            view = null;
        }

        public void UpdateView(MapElementData elementData)
        {
            view.Img_Element.rectTransform.localScale = Vector3.one * elementData.Scale;
            view.Img_Element.sprite = elementData.SpriteReference.Asset;
        }
        
    }

}
