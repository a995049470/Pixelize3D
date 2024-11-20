using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class EntityComponentArchive
    {
        public string Type;
        public string Data;

        public static EntityComponentArchive CreateArchive(EntityComponent component)
        {
            var archive = new EntityComponentArchive();
            archive.Type = component.GetType().FullName;
            (component as IComponentSaveOrLoadCallback)?.OnBeforeSave();
            archive.Data = JsonUtility.ToJson(component);
            return archive;
        }

        public void Load(EntityComponent component)
        {
            //先回收UID
            //TODO:思考EntityComponent的UID作用
            component.RecycleUniqueId();
            JsonUtility.FromJsonOverwrite(Data, component);
            (component as IComponentSaveOrLoadCallback)?.OnAfterLoad();
            component.UpdateReference();
        }
    }
}
