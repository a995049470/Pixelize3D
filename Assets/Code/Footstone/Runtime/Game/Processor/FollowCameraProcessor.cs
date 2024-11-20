
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class FollowCameraProcessor : SimpleGameEntityProcessor<FollowCameraComponent>
    {
        private CameraFollowPlayerProcessor cameraFollowPlayerProcessor;

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            cameraFollowPlayerProcessor  = GetProcessor<CameraFollowPlayerProcessor>();
        }

        public override void Update(GameTime time)
        {
            if(ComponentDatas.Count > 0)
            {
                var trans = cameraFollowPlayerProcessor.SingleComponent?.transform;
                var cameraPosition = trans ? trans.position : Vector3.zero;

                foreach (var kvp in ComponentDatas)
                {
                    var pos = kvp.Key.Entity.Transform.Position;
                    pos.x = cameraPosition.x;
                    pos.z = cameraPosition.z;
                    kvp.Key.Entity.Transform.Position = pos;
                }

            }
        }
    }
}
