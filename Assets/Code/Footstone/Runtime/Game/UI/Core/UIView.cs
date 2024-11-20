using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public abstract class UIView : EntityComponent
    {   
        private UIModel model;
        private RectTransform rectTransform;
        public RectTransform RectTransform { get => rectTransform; }

        protected override void Awake()
        {
            base.Awake();
            rectTransform = this.transform as RectTransform;
        }

        public UIModel GetOrCreateModel()
        {
            if(model == null)
            {
                model = CreateModel();
                model.Initialize(service);
            }
            return model;
        }
        

        protected abstract UIModel CreateModel(); 
    }
}
