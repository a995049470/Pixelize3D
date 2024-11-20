using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{


    [DefaultEntityComponentProcessor(typeof(MapProcessor))]
    public class MapComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [UnityEngine.HideInInspector]
        public MapData Data = new();

        protected override void Initialize(IServiceRegistry registry)
        {
            base.Initialize(registry);
            Data.Initialized(registry);
        }

        public void OnAfterLoad()
        {
            Data.OnAfterLoad();
        }

        public void OnBeforeSave()
        {
            Data.OnBeforeSave();
        }
    }

}



