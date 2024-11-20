using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPWaitAttackOnSleepProcessor : SimpleGameEntityProcessor<GOAPWaitAttackOnSleepComponent, GOAPAgentComponent>
    {
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var worldStatus = kvp.Value.Component2.WorldStatus;
                
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
