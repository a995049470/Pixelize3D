using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    /// <summary>
    /// 对包含 UIView组件的Entity 的 对象池
    /// </summary>
    public class UIViewEntityPool : EntityPool
    {
        public UIViewEntityPool(int _capacity) : base(_capacity)
        {
           
        }

        protected override void OnPop(Entity entity)
        {
            entity.Active = true;
        }

        protected override void OnPush(Entity entity)
        {
            entity.Active = false;
        }

        protected override void OnDestory(Entity entity)
        {
            UnityEngine.GameObject.Destroy(entity.gameObject);
        }
    }
}