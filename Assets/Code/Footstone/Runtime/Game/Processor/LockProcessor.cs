namespace Lost.Runtime.Footstone.Game
{

    public class LockProcessor : SimpleGameEntityProcessor<LockComponent>
    {
        public void Unlock(string key)
        {
            foreach (var kvp in ComponentDatas)
            {
                var lockComp = kvp.Value.Component1;
                if(lockComp.IsLocked && lockComp.LockName == key)
                {
                    lockComp.IsLocked = false;
                    break;
                }
            }
        }
    }

}
