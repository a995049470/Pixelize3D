using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(ChestStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(InteractivePickChestProcessor))]
    //[DefaultEntityComponentProcessor(typeof(InteractionChestProcessor))]
    public class ChestComponent : EntityComponent
    {
        [UnityEngine.HideInInspector]
        public bool IsOpen = false;
        public bool IsPickable = true;
        public string ChestKey = "chest";
        public string TipKey = "";
        [UnityEngine.HideInInspector]
        public bool IsSendInteractiveRequest = false;
        

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            IsOpen = false;
            IsSendInteractiveRequest = false;
        }
    }

}
