using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class InteractionProcessor : SimpleGameEntityProcessor<InteractorComponent, RotateComponent>
    {
        //成功探测到交互物体的
        //public HashSet<InteractorComponent> SuccessInteractors = new();
        private TipProcessor tipProcessor;

        public InteractionProcessor() : base()
        {
            Order = ProcessorOrder.Interaction;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            tipProcessor = GetProcessor<TipProcessor>();
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var interactorComp = kvp.Value.Component1;
                var rotateComp = kvp.Value.Component2;
                var position = interactorComp.Entity.Transform.Position;
                var dir = rotateComp.FaceDirection;
                var start = position;
                var end = position + dir;
                var endV2I = new Vector2Int(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.z));
                bool isAutoCheck = endV2I != interactorComp.LastAutoCheckPosition;
                //if (interactorComp.IsInteracting || isAutoCheck)
                {
                    var castCount = physicsSystem.RaycastNonAlloc(start, end, raycastHits, GameConstant.InteractionLayer);
                    //bool isAutoCheckSuccess = false;
                    bool isSuccessCheck = false;
                    if(castCount > 0)
                    {
                        for (int i = 0; i < castCount; i++)
                        {
                            var collider = raycastHits[i].collider;
                            var target = collider.GetComponent<TargetEntity>()?.Target;
                            var interactiveComp = target?.Get<InteractiveComponent>();
                            if(interactiveComp && interactiveComp.IsWaitTrigger)
                            {
                                isSuccessCheck = true;
                                if(interactorComp.IsInteracting)
                                {
                                    var entity = interactorComp.Entity;
                                    cmd.AddEntityComponent<InteractiveLabelComponent>(target, ownerComp =>
                                    {
                                        ownerComp.Target = ownerComp.Target ?? entity;
                                    });
                                }
                                else if(isAutoCheck)
                                {
                                    //isAutoCheckSuccess = true;
                                }
                                break;
                            }
                        }
                    }
                    // if(isAutoCheckSuccess) SuccessInteractors.Add(interactorComp);
                    // else SuccessInteractors.Remove(interactorComp);

                    interactorComp.LastAutoCheckPosition = endV2I;
                    interactorComp.IsInteracting = false;
                    var uid = interactorComp.Id;
                    if(isSuccessCheck)
                    {   
                        var headPosition = interactorComp.TipPointReference.Component.position;
                        tipProcessor.SingleComponent.UpdateInteractiveTipInfo(uid, headPosition, time.DeltaTime);
                    }
                    else
                    {
                        tipProcessor.SingleComponent.ClearInteractiveTipInfo(uid);
                    }
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }

        protected override void OnEntityComponentRemoved(Entity entity, InteractorComponent component, GameData<InteractorComponent, RotateComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);
            //SuccessInteractors.Remove(component);
        }
    }
}
