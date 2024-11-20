using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(BuildPlotProcessor))]
    [DefaultEntityComponentProcessor(typeof(PlayerBuildPlotControllerProcessor))]
    [DefaultEntityComponentProcessor(typeof(BuildPlotUpdateActionMaskProcessor))]
    public class BuildPlotComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        public PowerReceicver Receicver = new();
        public bool IsBuilding = false;
        [HideInInspector]
        public int BagGridIndex;
        [HideInInspector]
        public ulong ItemUID;
        [HideInInspector]
        public Vector3 BuildDir;
        [HideInInspector]
        public Vector3 BuildPosition;
        [HideInInspector]  
        public string PlotKey;

        public void StartBuilding(string key, int girdId, ulong itemUID, Vector3 dir, Vector3 buildPos)
        {
            PlotKey = key;
            IsBuilding = true;
            BagGridIndex = girdId;
            ItemUID = itemUID;
            BuildDir = dir;
            BuildPosition = buildPos;
        }

        public void ForceCompleteBuild()
        {
            IsBuilding = false;
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



