using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 可以拷贝
    /// </summary>
    [DefaultEntityComponentProcessor(typeof(AutoRecycleCopyableProcessor))]
    public class CopyLabelComponent : EntityComponent
    {
        //生效过？ 生效以后才需要回收
        [UnityEngine.HideInInspector]
        public bool IsEffect = false;
        [SerializeField][HideInInspector]
        private ulong uid;
        public Entity Target
        {
            set
            {
                if(value) uid = value.Id;
                else uid = uniqueIdManager.InvalidId;
            }
            get
            {
                sceneSystem.SceneInstance.TryGetEntity(uid, out var entity);
                return entity;
            }
        }
        public void Copy(Entity entity)
        {
            Target = entity;
            IsEffect = false;
        }

        
    }
}
