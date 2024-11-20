using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //实体数据 NoOverwrite
    [SerializeField]
    public class EntityData
    {
        public string Key;
        public ResFlag Flag;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 LocalScale;

        public static EntityData Create(string key, ResFlag flag, GameObject entityGo)
        {
            EntityData data = new();
            data.Key = key;
            data.Flag = flag;
            data.Position = entityGo.transform.position;
            data.Rotation = entityGo.transform.rotation;
            data.LocalScale = entityGo.transform.localScale;
            return data;

        }

        public Entity Load(ResPoolManager resPoolManager)
        {
            var entity = resPoolManager.InstantiateEntity(Key, Flag);
            entity.Transform.Position = Position;
            entity.Transform.Rotation = Rotation;
            entity.Transform.LocalScale = LocalScale;
            return entity;
        }
    }
}
