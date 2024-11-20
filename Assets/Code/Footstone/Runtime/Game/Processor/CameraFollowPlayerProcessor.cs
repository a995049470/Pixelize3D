using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class CameraFollowPlayerProcessor : SimpleGameEntityProcessor<CameraFollowPlayerComponent, TransformComponent>
    {
        public CameraFollowPlayerComponent SingleComponent;
        private PlayerProcessor playerProcessor;
        //摄像头瞬移
        private bool isCameraTransport = false;
        public CameraFollowPlayerProcessor() : base()
        {
            Order = ProcessorOrder.FollowPlayer;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            playerProcessor = GetProcessor<PlayerProcessor>();
        }
        
        public void OnEntityTransport(Entity entity)
        {
            if(playerProcessor == null) playerProcessor = GetProcessor<PlayerProcessor>();
            isCameraTransport |= entity == playerProcessor.Target.Entity;
        }

        protected override void OnEntityComponentAdding(Entity entity, CameraFollowPlayerComponent component, GameData<CameraFollowPlayerComponent, TransformComponent> data)
        {
            base.OnEntityComponentAdding(entity, component, data);
            if(SingleComponent == null) SingleComponent = component;
        }

        protected override void OnEntityComponentRemoved(Entity entity, CameraFollowPlayerComponent component, GameData<CameraFollowPlayerComponent, TransformComponent> data)
        {
            base.OnEntityComponentRemoved(entity, component, data);

            if(SingleComponent == component)
            {
                foreach (var kvp in ComponentDatas)
                {
                    if(SingleComponent != kvp.Key)
                    {
                        SingleComponent = kvp.Key;
                        break;
                    }
                }
                if(SingleComponent == component) SingleComponent = null;
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            float totalTime = (float)time.Elapsed.TotalSeconds;
            float scale = 1.0f + (input.MouseWheelDelta * 0.05f);

            var target = playerProcessor.Target;
            if (target != null)
            {
                foreach (var kvp in ComponentDatas)
                {
                    var followPlayerComp = kvp.Value.Component1;
                    var transComp = kvp.Value.Component2;
                    var viewDir = transComp.Forward;
                    var targetPos = target.Position + followPlayerComp.TargetOffset - viewDir * followPlayerComp.Distance;
                    followPlayerComp.Distance *= scale;
                    var currentPos = transComp.Position;
                    if(isCameraTransport)
                    {
                        isCameraTransport = false;
                        transComp.Position = targetPos;
                        followPlayerComp.CurrentVelocity = Vector3.zero;
                    }
                    else
                    {
                        var finalPos = MathHelper.SmoothDamp(currentPos, targetPos, ref followPlayerComp.CurrentVelocity, followPlayerComp.SmoothTime, followPlayerComp.MaxSpeed, totalTime);
                        transComp.Position = finalPos;
                    }
                };
            }
        }


    }
}
