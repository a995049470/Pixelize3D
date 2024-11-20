using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(CraftProcessor))]
    public class CraftComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [UnityEngine.HideInInspector]
        public CraftData Data = new();

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



