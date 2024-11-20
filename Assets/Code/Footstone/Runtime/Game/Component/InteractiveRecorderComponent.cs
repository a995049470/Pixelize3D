using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //独特触发物记录组件
    [DefaultEntityComponentProcessor(typeof(InteractiveRecorderProcessor))]
    public class InteractiveRecorderComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [SerializeField][HideInInspector]
        private SerializableHashSet<string> triggeredInteractiveKeys = new();

        public bool IsUniqueTriggered(string key)
        {
            return triggeredInteractiveKeys.Contains(key);
        }

        public void UniqueTriggered(string Key)
        {
            triggeredInteractiveKeys.Add(Key);
        }

        public void OnAfterLoad()
        {
            triggeredInteractiveKeys.OnAfterLoad();
        }

        public void OnBeforeSave()
        {
            triggeredInteractiveKeys.OnBeforeSave();
        }
    }

}



