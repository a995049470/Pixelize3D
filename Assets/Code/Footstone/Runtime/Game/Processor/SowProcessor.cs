using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class SowProcessor : SimpleGameEntityProcessor<SowComponent, ActionMaskComponent, RotateComponent>
    {
        private PlaceProcessor placeProcessor;
        //10°的cos值
        public SowProcessor() : base()
        {
            Order = ProcessorOrder.Sow;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            placeProcessor = GetProcessor<PlaceProcessor>();
        }

        private bool IsCorrectFaceDir(Vector3 faceDir, Vector3 attackDir)
        {
            return DirectionUtil.IsCorrectFaceDir(faceDir, attackDir);
        }

        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var sowComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(sowComp.IsSowing)
                {
                    var rotateComp = kvp.Value.Component3;
                    bool isSowUpdate = true;
                    bool isInvokeSow = false;
                    if(!actionMaskComp.IsActionEnable(ActionFlag.Sow) || 
                       rotateComp.FaceDirection != sowComp.SowDir)
                    {
                        sowComp.ForceCompleteSow();
                        isSowUpdate = false;
                    }
                    
                    if(!IsCorrectFaceDir(sowComp.Entity.Transform.Forward, sowComp.SowDir))
                    {
                        isSowUpdate = false;
                    }
                    if(isSowUpdate)
                    {
                        sowComp.SowUpdate(time.DeltaTime, out isInvokeSow, out var effectKey);
                    }
                    if(isInvokeSow)
                    {
                        sceneSystem.SceneInstance.TryGetEntity(sowComp.CroplandUID, out var entity);
                        if(entity != null)
                        {
                            var position = entity.transform.position;
                            bool isEmpty = placeProcessor.IsEmpty(position);
                            if(isEmpty && bagData.TryLoseItem(sowComp.BagGridIndex, sowComp.ItemUID, sowComp.Count))
                            {
                                var plant = sowComp.Plant;
                                cmd.InstantiateEntity(plant, ResFlag.Entity_Plant, entity.Transform.Position, 0);
                            }
                            
                        }
                    }      
                }
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
