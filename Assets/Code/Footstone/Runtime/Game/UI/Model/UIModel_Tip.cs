using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class UIModel_Tip : UIModel
    {
        private UIView_Tip view;
        public int TargetGridPos;
        public UIModel_Tip(UIView view) : base(view)
        {
            
        }

        public override void UpdateView(GameTime time)
        {
            bool isHide = true;
            if(TargetGridPos >= 0)
            {
                var gridData = bagData.GetBagGridItemData(TargetGridPos);
                if(gridData.IsVaild())
                {
                    isHide = false;
                    SetActive(true);
                    view.Text_Name.text = gridData.GetItemName();
                }
            }
            if(isHide) SetActive(false);
        }


        public override void BindView()
        {
            view = OriginView as UIView_Tip;
        }

        public override void UnbindView()
        {
            
        }
    }

}
