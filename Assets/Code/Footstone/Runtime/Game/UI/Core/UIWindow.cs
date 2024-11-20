namespace Lost.Runtime.Footstone.Game
{
    public abstract class UIWindow : UIModel
    {
        protected UIWindow(UIView view) : base(view)
        {

        }
        public abstract int Orderer { get; }
        public virtual bool AllowGameInput { get; } = false;

        //成为置顶
        public virtual void OnTop()
        {

        }

        //被覆盖
        public virtual void OnCovered()
        {

        }
    }
}
