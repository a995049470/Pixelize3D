using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class PickProcessor : SimpleGameEntityProcessor<PickComponent>
    {
        
        public PickProcessor() : base()
        {
            Order = ProcessorOrder.Pick;
            
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var pickComp = kvp.Value.Component1;
                if(pickComp.IsPicking)
                {
                    pickComp.IsPicking = false;
                    var entity = pickComp.Entity;
                    var position = entity.Transform.Position;
                    var radius = 0.5f;
                    var castNum = physicsSystem.SphereCastNonAlloc(position, radius, castColliders, GameConstant.PickableLayer);
                    for (int i = 0; i < castNum; i++)
                    {
                        var targetEntity = castColliders[i].GetComponent<TargetEntity>()?.Target;
                        //var pickableComp = targetEntityComp?.Target?.GetOrCreate<PickLableComponent>();
                        cmd.AddEntityComponent<PickLabelComponent>(targetEntity, comp =>
                        {
                            comp.Pick(entity);
                        });
                        
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
