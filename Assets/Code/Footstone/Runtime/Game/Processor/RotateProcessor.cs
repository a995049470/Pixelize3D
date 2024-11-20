using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class RotateProcessor : SimpleGameEntityProcessor<RotateComponent, TransformComponent, ActionMaskComponent>
    {
        public RotateProcessor() : base()
        {
            Order = ProcessorOrder.Rotate;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                
                var data = kvp.Value;
                var rotate = data.Component1;
                var rotateSpeedRad = rotate.GetRotateSpeedRad(time.FrameCount);
                var actionMaskComp = data.Component3;
                //初始化面向方向
                if(rotate.FaceDirection == Vector3.zero)
                {
                    if(rotate.IsRandomDirection)
                    {
                        var dirs = new Vector3[4]
                        {
                            new Vector3(-1, 0, 0),
                            new Vector3(1, 0, 0),
                            new Vector3(0, 0, -1),
                            new Vector3(0, 0, -1)
                        };
                        var dir = dirs[random.Next(0, 4)];
                        rotate.Entity.Transform.Rotation = Quaternion.LookRotation(dir, Vector3.up);
                        rotate.FaceDirection = dir;
                    }
                    else
                    {
                        rotate.FaceDirection = DirectionUtil.CalculateDirection(rotate.Entity.Transform.Forward);
                    }
                }
                else if (actionMaskComp.IsActionEnable(ActionFlag.Rotate))
                {
                    var dir = rotate.FaceDirection;
                    //TODO:确认这个forward方向是否正确
                    var transComp = data.Component2;
                    var forward = transComp.Forward;
                    forward.Normalize();
                    var fDotD = Mathf.Clamp(Vector3.Dot(forward, dir), -1, 1);
                    var angle = Mathf.Acos(fDotD);
                    var rotateAngle = rotateSpeedRad * time.DeltaTime;

                    if (angle < rotateAngle)
                    {
                        transComp.Rotation = Quaternion.LookRotation(dir, Vector3.up);
                    }
                    else
                    {
                        rotateAngle = Mathf.Min(angle, rotateSpeedRad * time.DeltaTime);
                        var rotateDir = Vector3.Cross(forward, dir).y >= 0 ? 1 : -1;
                        transComp.Rotation = Quaternion.AngleAxis(rotateAngle * rotateDir * Mathf.Rad2Deg, Vector3.up) * transComp.Rotation;
                    }
                }
            }
        }

       
    }
}
