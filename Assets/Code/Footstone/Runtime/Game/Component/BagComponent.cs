using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(BagProcessor))]
    public class BagComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        public BagData Data = new();

        public void OnAfterLoad()
        {
            Data.AfterLoad(service);
        }

        public void OnBeforeSave()
        {
            
        }

        protected override void OnEnableRuntime()
        {
            Data.Initialized(service);
        }
        
    }

}



