using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPAgentProcessor : SimpleGameEntityProcessor<GOAPAgentComponent>
    {
        private GOAPAstar astar = new();
        public GOAPAgentProcessor() : base()
        {
            Order = ProcessorOrder.AIThink;
        }

        
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var agentComp = kvp.Value.Component1;
                var entity = agentComp.Entity;
                bool isTaskCompelte = agentComp.Task.TryCompleteTask(cmd, entity, agentComp.ActionNodes, agentComp.WorldStatus);
                if(isTaskCompelte)
                {
                    astar.TryFindNodeIndiceToComplteGoal(agentComp);
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        

    }
}
