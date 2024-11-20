using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{


    public class EntityPool : Pool<Entity>
    {
        public EntityPool(int _capacity) : base(_capacity)
        {

        }

        protected override void OnPop(Entity t)
        {
            t.Active = true;
        }

        protected override void OnPush(Entity t)
        {
            t.Transform.Parent = null;
            t.Active = false;
        }

        protected override void OnDestory(Entity t)
        {
            t.DestoryUnityGameObject();
        }

    }
}