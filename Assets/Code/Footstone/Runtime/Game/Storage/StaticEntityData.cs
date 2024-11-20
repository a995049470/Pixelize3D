using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 组件数据固定的实体 的 储存数据
    /// </summary>
    [System.Serializable]
    public class StaticEntityData
    {
        public ResFlag Flag;
        public string Key;
        public Vector3 Position;
        public Quaternion Rotation;
        
        public StaticEntityData()
        {

        }

        public StaticEntityData(TagComponent tagComp)
        {
            var transComp = tagComp.Entity.Transform;
            Flag = tagComp.Flag;
            Key = tagComp.Key;
            Position = transComp.Position;
            Rotation = transComp.Rotation;
        }

        public void OnLoad(ResPoolManager resPoolManager)
        {
            var entity = resPoolManager.InstantiateEntity(Key, Flag);
            entity.Transform.Position = Position;
            entity.Transform.Rotation = Rotation;
            entity.InvokeEntityInstantiateCallback();
        }
    }
}
