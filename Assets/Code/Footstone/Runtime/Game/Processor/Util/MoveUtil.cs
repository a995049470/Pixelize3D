using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;
using Random = System.Random;

namespace Lost.Runtime.Footstone.Game
{

    public static class MoveUtil
    {
        public static bool IsNoBarrierOnWay(PhysicsSystem physicsSystem, Vector3 from, Vector3 to)
        {
            return !physicsSystem.Raycast(from, to, GameConstant.Layer0 | GameConstant.BarrierLayer);
        }

        public static void MoveStart(VelocityComponent velocity, RotateComponent rotate, ActionMaskComponent actionMask, PhysicsSystem physicsSystem, VelocityProcessor velocityProcessor, Vector3 dir)
        {
            var isIdle = velocity.InvaildTargetPos;
            var hasEngoughEnery = true;
            var isZeroDir = dir == Vector3.zero;
            if (isIdle)
            {
                if (isZeroDir)
                {
                    velocity.BackToTargetPos();
                }
                else if (hasEngoughEnery)
                {
                    var from = velocity.TargetPos;
                    var to = velocity.TargetPos + dir;
                    var isVaildTargetPos = true;

                    isVaildTargetPos = !physicsSystem.Raycast(from, to, GameConstant.Layer0 | GameConstant.BarrierLayer) && velocityProcessor.TryMove(from, to);
                    
                    if (isVaildTargetPos)
                    {
                        //变向 偿还多移动的距离
                        if (rotate.FaceDirection != dir)
                        {
                            velocity.BackToTargetPos();
                        }
                        rotate.FaceDirection = dir;
                        velocity.TargetPos = to;
                        actionMask.DisabledAction(ActionFlag.Attack);
                    }
                    else
                    {
                        rotate.FaceDirection = dir;
                    }
                }
            }
            //强行转向
            else if (!isZeroDir)
            {
                var d = Vector3.Dot(velocity.TargetPos - velocity.MoveStartPos, dir);
                var isTurn180 = d < -0.5f;
                var isTurn90 = Mathf.Abs(d) < 0.1f; 
                if (isTurn180 && velocityProcessor.TryMove(velocity.TargetPos, velocity.MoveStartPos))
                {
                    //energyComp.TryLoseEnerge(ctrlComp.EnergyCost);
                    var temp = velocity.MoveStartPos;
                    velocity.MoveStartPos = velocity.TargetPos;
                    
                    velocity.TargetPos = temp;
                    rotate.FaceDirection = dir;
                }
                else if(isTurn90)
                {
                    var currentPosition = velocity.Entity.Transform.Position;
                    var dis = Vector3.Dot(velocity.TargetPos - velocity.MoveStartPos, currentPosition - velocity.MoveStartPos);
                    if(dis > 0.7f)
                    {
                        rotate.FaceDirection = dir;
                    }
                }
            }
        }

        /// <summary>
        /// 尝试随机获取下一个移动终点
        /// </summary>
        /// <param name="directions">0:180度转 1:90度转 2:不转  3种情况的权重</param>
        /// <param name="weights">所有可能的转向方向</param>
        public static bool TryRandomGetNextMoveDir(Vector3 origin, Vector3 faceDir, Vector3[] directions, int[] weights, PhysicsSystem physicsSystem, VelocityProcessor velocityProcessor, Random random, out Vector3 nextMoveDir)
        {
            List<(Vector3, int)> possbileDirs = new List<(Vector3, int)>();
            int totalWeight = 0;
            for (int i = 0; i < directions.Length; i++)
            {
                var dir = directions[i];
                var point = origin + dir;

                var from = origin;
                var to = origin + dir;
                var isValidTargetPos = !physicsSystem.Raycast(from, to, GameConstant.Layer0 | GameConstant.BarrierLayer) && velocityProcessor.IsVaildTargetPos(to);
                if (isValidTargetPos)
                {
                    //根据面向方向决定下次行动的方法.
                    int d = (int)MathF.Round(Vector3.Dot(faceDir, dir)) + 1;
                    var weight = weights[d];
                    totalWeight += weight;
                    var item = (dir, weight);
                    possbileDirs.Add(item);
                }
            }
            nextMoveDir = Vector3.zero;
            bool isSuccess = possbileDirs.Count > 0;
            if (isSuccess)
            {
                var r = random.Next(0, totalWeight);
                var taretId = 0;
                for (taretId = 0; taretId < possbileDirs.Count; taretId++)
                {
                    r -= possbileDirs[taretId].Item2;
                    if (r < 0) break;
                }
                nextMoveDir = possbileDirs[taretId].Item1;
            }
            return isSuccess;
        }


        public static bool IsMoveable(ActionMaskComponent actionMaskComp, VelocityComponent velocityComp)
        {
            return actionMaskComp.IsActionEnable(ActionFlag.Move) && velocityComp.InvaildTargetPos;
        }

        public static Vector3 CaluteMoveDir(Vector3 start, Vector3 end)
        {
            return (end - start).normalized;
        }

        public static bool IsVaildMoveTarget(Vector3 target, VelocityProcessor velocityProcessor, AStarSystem aStarSystem, PlotProcessor plotProcessor)
        {
            bool isVaildTarget = velocityProcessor.IsVaildTargetPos(target) && 
            plotProcessor.IsPlot(target) &&
            aStarSystem.IsWayPoint(target);
            return isVaildTarget;
        }
    }
}

