using Lost.Runtime.Footstone.Core;
using UnityEngine;
namespace Lost.Runtime.Footstone.Game
{

    public class GOAPPlayerSensorProcessor : SimpleGameEntityProcessor<GOAPPlayerSensorComponent, GOAPAgentComponent, TransformComponent>
    {
        private PlayerProcessor playerProcessor;

        private bool IsInView(Vector3 origin, Vector3 target, Vector3 forward, float distance, float maxDistance, float fov)
        {
            bool inView = false;
            if(distance < maxDistance && distance > 0)
            {
                var dir = (target - origin).normalized;
                inView = Vector3.Dot(dir, forward) > Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad);
                if(inView) inView &= !physicsSystem.Raycast(origin, target, GameConstant.BarrierLayer);
            }
            return inView;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            playerProcessor = GetProcessor<PlayerProcessor>();
        }

        public override void Update(GameTime time)
        {
            
            foreach (var kvp in ComponentDatas)
            {
                var worldStatus = kvp.Value.Component2.WorldStatus;
                var transComp = kvp.Value.Component3;
                var sensorComp = kvp.Value.Component1;
                var isNew = worldStatus.ForceGetStatus(GOAPStatusFlag.DiscoverPlayer, out var status);
                bool isDiscoverPlayer = status.GetBool();
                var playerPosition = playerProcessor.Target.Position;
                var currentPositon = transComp.Position;
                var distance = Vector3.Distance(playerPosition, currentPositon);
                if(isDiscoverPlayer)
                {
                    isDiscoverPlayer = distance < sensorComp.LoseHatredMaxRadius;
                }
                else 
                {
                    bool isHurtRecenty = worldStatus.GetFloat(GOAPStatusFlag.LastHurtTime, GameConstant.FloatScale, 99.0f) < 1.0f;
                    //受伤直接锁定敌人？
                    if(isHurtRecenty) isDiscoverPlayer = true;
                    else isDiscoverPlayer = IsInView(currentPositon, playerPosition, transComp.Forward, distance, sensorComp.ReciveHatredMaxRadius, sensorComp.FOV);
                }
                status.Set(isDiscoverPlayer);
                worldStatus.Set(GOAPStatusFlag.AttackTargetDistance, distance, GameConstant.FloatScale);
            }
        }
    }
}
