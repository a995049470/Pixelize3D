using LitJson;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //固定行为组件
    [DefaultEntityComponentProcessor(typeof(GOAPFixedMoveRotateProcessor))]
    public class GOAPFixedBehaviorComponent : EntityComponent
    {
        public string BehaviourKey = "";
        public int BehaviorPtr = 0;
        private bool isDitryKey = true;
        private JsonData cacheData;
        
        public void SetBehaviourKey(string key)
        {
            BehaviourKey = key;
            isDitryKey = true;
            BehaviorPtr =  0;
        }

        public JsonData GetBehaviourData(ResPoolManager resPoolManager)
        {
            if(isDitryKey)
            {
                isDitryKey = false;
                cacheData = resPoolManager.LoadJsonData(BehaviourKey, ResFlag.Text_FixedBehavior);
            }
            return cacheData;
        }

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            isDitryKey = true;
        }

        public void Reset()
        {
            BehaviorPtr = 0;
        }
    }

}



