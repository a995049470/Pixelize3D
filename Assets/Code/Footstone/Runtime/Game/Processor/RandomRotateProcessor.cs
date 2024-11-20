using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class RandomRotateProcessor : SimpleGameEntityProcessor<RandomRotateComponent>
    {
        public RandomRotateProcessor() : base()
        {
            Order = ProcessorOrder.FrameStart;
        }

        public override void Update(GameTime time)
        {
            if(ComponentDatas.Count > 0)
            {
                var cmd = commandBufferManager.Get();
                foreach (var kvp in ComponentDatas)
                {
                    var randomRotateComp = kvp.Value.Component1;
                    var angle = random.Next(0, 360) / randomRotateComp.Interval * randomRotateComp.Interval;
                    var qua = Quaternion.AngleAxis(angle, Vector3.up);
                    var transComp = randomRotateComp.Entity.Transform;
                    transComp.Rotation = qua * transComp.Rotation;
                    cmd.RemoveEntityComponent(randomRotateComp);
                }
                cmd.Execute();
                commandBufferManager.Release(cmd);
            }
        }
    }
}
