using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(PlayerFastUseControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(FastUseProcessor))]
    public class FastUseComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [HideInInspector]
        public PowerReceicver Receicver = new();
        [HideInInspector]
        public Vector3 FastUseDir;
        [HideInInspector]
        public bool IsFastUsing = false;
        [HideInInspector]
        public int BagGridIndex;
        [HideInInspector]
        public ulong ItemUID;
        

        public void StartFastUse(Vector3 dir, int gridIndex, ulong itemUID)
        {
            IsFastUsing = true;
            FastUseDir = dir;
            BagGridIndex = gridIndex;
            ItemUID = itemUID;
        }

        public void ForceCompleteFastUse()
        {
            IsFastUsing = false;
        }

        public void OnAfterLoad()
        {
            Receicver.OnAfterLoad();
        }

        public void OnBeforeSave()
        {
            Receicver.OnBeforeSave();
        }
    }

}



