using Lost.Runtime.Footstone.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(RepulsedProcessor))]
    [DefaultEntityComponentProcessor(typeof(RepulsedUpdateActionMaskProcessor))]
    public class RepulsedComponent : EntityComponent
    {
        [SerializeField][HideInInspector]
        private float repulsedSpeed = 10;
        [SerializeField][HideInInspector]
        private Vector3 repulsedDir = Vector3.forward;
        [SerializeField][HideInInspector]
        private float repulsedDis = 1;
        [SerializeField][HideInInspector]
        private float remainRepulsedDis = 1;
        [SerializeField][HideInInspector]
        private Vector3 repulsedTargetPosition;
        [SerializeField][HideInInspector]
        private Vector3 repulsedStart;
        private bool isVaildRepulsedStart;

        public void StartRepulsed(Vector3 start, float speed, float dis, Vector3 defaultDir)
        {
            repulsedStart  = start;
            repulsedDir = defaultDir;
            repulsedDis = dis;
            remainRepulsedDis = dis;
            repulsedSpeed = speed;
            repulsedDir = DirectionUtil.CalculateDirection(this.Entity.Transform.Position - repulsedStart, repulsedDir);
            isVaildRepulsedStart = true;
        }

        public void StartRepulsed(float speed, float dis, Vector3 defaultDir)
        {
            repulsedDir = defaultDir;
            repulsedDis = dis;
            remainRepulsedDis = dis;
            repulsedSpeed = speed;
            repulsedDir = defaultDir;
            isVaildRepulsedStart = false;
        }

        public bool TryRepulsed(GameTime time, VelocityComponent velocityComp, VelocityProcessor velocityProcessor, PhysicsSystem physicsSystem)
        {
            bool isContinue = remainRepulsedDis > 0;
            if(isContinue)
            {
                if(!isVaildRepulsedStart)
                {
                    repulsedStart = velocityComp.TargetPos;
                    isVaildRepulsedStart = true;
                }
                var currentTarget = repulsedStart + repulsedDir * (repulsedDis - remainRepulsedDis + 1);
                if(currentTarget != velocityComp.TargetPos)
                {
                    if(velocityProcessor.TryMove(velocityComp.TargetPos, currentTarget))
                    {
                        velocityComp.SetVaildTargetPos(currentTarget);
                    }
                    else
                    {
                        isContinue = false;
                    }
                }
                if(isContinue)
                {
                    var pos = velocityComp.Entity.Transform.Position;
                    var remainDis = Vector3.Dot(currentTarget - pos, repulsedDir);
                    var t = repulsedSpeed * time.DeltaTime * repulsedDir;
                    bool isArriveTarget = t.magnitude >= remainDis;
                    if(isArriveTarget) remainRepulsedDis --;
                    
                    if(remainRepulsedDis == 0)
                    {
                        isContinue = false;
                        velocityComp.Entity.Transform.Position = currentTarget;
                    }
                    else
                    {
                        velocityComp.Entity.Transform.Position += t;
                    }
                }
            }
            
            
            return isContinue;
        }

      
        
    }

}
