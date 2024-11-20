using System;
using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(TimeProcessor))]
    public class TimeComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [UnityEngine.HideInInspector]
        public SerializableDictionary<string, int> SceneCostPowerDic = new();
        public int TotalGrowthPower = 0;
        public int Day = 0;

        public void OnAfterLoad()
        {
            SceneCostPowerDic.OnAfterLoad();
        }

        public void OnBeforeSave()
        {
            SceneCostPowerDic.OnBeforeSave();
        }
    }

}