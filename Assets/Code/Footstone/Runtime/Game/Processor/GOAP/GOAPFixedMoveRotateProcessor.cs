using LitJson;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    using GOAPFixedMoveRotateData = GameData<GOAPFixedBehaviorComponent, GOAPAgentComponent, VelocityComponent, ActionMaskComponent, RotateComponent>;

    //处理AI固定的移动旋转行为
    public class GOAPFixedMoveRotateProcessor : SimpleGameEntityProcessor<GOAPFixedBehaviorComponent, GOAPAgentComponent, VelocityComponent, ActionMaskComponent, RotateComponent>
    {
        private MapPointProcessor mapPointProcessor;
        private VelocityProcessor velocityProcessor;
        
        public GOAPFixedMoveRotateProcessor() : base()
        {
            Order = ProcessorOrder.AICommand;
        }

        public override void BeforeProcessorRunTheFirstFrame()
        {
            base.BeforeProcessorRunTheFirstFrame();
            mapPointProcessor = GetProcessor<MapPointProcessor>();
            velocityProcessor = GetProcessor<VelocityProcessor>();
        }

        private void Move(JsonData behavior, GOAPFixedMoveRotateData data)
        {
            bool isSuccess = false;
            var key_point = (string)behavior[JsonKeys.point];
            bool isActionFinish = !mapPointProcessor.TryGetTargetPoint(key_point, out var targetPosition);
            if (!isActionFinish)
            {
                var velocityComp = data.Component3;
                bool isArraive = velocityComp.IsArrive(targetPosition);
                //寻路成功行为 成功
                if(isArraive)
                {
                    isActionFinish = true;
                    isSuccess = true;
                }
                else if(!velocityComp.IsMoving)
                {
                    var startPosition = velocityComp.MoveStartPos;
                    bool isFind = aStarSystem.TryFindNextPoistion(startPosition, targetPosition, 0, out var nextPosition);
                    if(isFind)
                    {
                        var dir = MoveUtil.CaluteMoveDir(startPosition, targetPosition);
                        var rotateComp = data.Component5;
                        var actionMask = data.Component4;
                        MoveUtil.MoveStart(velocityComp, rotateComp, actionMask, physicsSystem, velocityProcessor, dir);
                    }
                    //寻路失败 行为失败
                    else
                    {
                        isActionFinish = true;
                        isSuccess = false;
                    }
                }
            }
            if(isActionFinish)
            {
                var actioComp = data.Component1;
                var agentComp = data.Component2;
                if(isSuccess) actioComp.BehaviorPtr += 1;
                else agentComp.Task.FinshCurrentAction(false);
            }
        }

        private void Face(JsonData behavior, GOAPFixedMoveRotateData data)
        {
            var dir = LitJsonUtil.ToVector3Int(behavior[JsonKeys.dir]);
            var rotateComp = data.Component5;
            rotateComp.FaceDirection = dir;
            bool isActionFinish = !rotateComp.IsRotating();
            var actioComp = data.Component1;
            if(isActionFinish) actioComp.BehaviorPtr += 1;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            foreach (var kvp in ComponentDatas)
            {
                var behaviorComp = kvp.Value.Component1;
                var behaviorData = behaviorComp.GetBehaviourData(resPoolManager);
                var agentComp = kvp.Value.Component2;
                bool isFinish = behaviorComp.BehaviorPtr >= behaviorData.Count;
                if(!isFinish)
                {
                    var singleBehaviorData = behaviorData[behaviorComp.BehaviorPtr];
                    var name = (string)singleBehaviorData[JsonKeys.name];
                    if(name == GameConstant.FixedBehavior_Move)
                    {
                        Move(singleBehaviorData, kvp.Value);
                    }
                    else if(name == GameConstant.FixedBehavior_Face)
                    {
                        Face(singleBehaviorData, kvp.Value);
                    }
                }
                else
                {
                    agentComp.Task.FinshCurrentAction(true);
                }
              
            }
        }

        

        
    }
}
