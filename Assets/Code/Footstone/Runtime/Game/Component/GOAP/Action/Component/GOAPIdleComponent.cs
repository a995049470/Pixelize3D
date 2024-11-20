using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    //在原地但是可能会旋转
    [DefaultEntityComponentProcessor(typeof(GOAPIdleProcessor))]
    public class GOAPIdleComponent : EntityComponent
    {
        public bool IsReceiveAttack = false;
        public Vector2 RotateIntervalRange = new Vector2(2.0f, 5.0f);
        [HideInInspector]
        public float RotateTimer = 0;
        [HideInInspector]
        public float NextRotateTimePoint = 0;

        public void Reset()
        {
            IsReceiveAttack = false;
        }

        public bool TryRotate(System.Random random, float deltaTime, out float rotateAngle)
        {
            bool isRotate = false;
            rotateAngle = 0;
            if(RotateTimer >= NextRotateTimePoint)
            {
                NextRotateTimePoint += RandomUtil.Next(random, RotateIntervalRange);
            }
            
            RotateTimer += deltaTime;

            if(NextRotateTimePoint > 0 && RotateTimer >= NextRotateTimePoint)
            {
                isRotate = true;
                rotateAngle = 90;
            }
            return isRotate;
        }

    }

}



