using Lost.Runtime.Footstone.Core;



using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(MoveProcessor))]
    [DefaultEntityComponentProcessor(typeof(WalkStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(VelocityUpdateActionMaskProcessor))]
    [DefaultEntityComponentProcessor(typeof(RemoveVelocityComponentOnDeadProcessor))]
    [DefaultEntityComponentProcessor(typeof(UpdateIdleSubIndexProcessor))]
    [DefaultEntityComponentProcessor(typeof(VelocityProcessor))]
    public class VelocityComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [SerializeField]
        public CharacterAttribute Speed = new();

        //TargetPos和MoveStartPos是否为有效坐标
        [SerializeField][HideInInspector]
        private bool isVaildTargetPos = false;
        [HideInInspector]
        public Vector3 Direction;
        [SerializeField][HideInInspector]
        private Vector3 targetPos;
        public Vector3 TargetPos
        {
            get
            {
                if(!isVaildTargetPos)
                {
                    SetVaildTargetPos(this.Entity.Transform.Position);
                }
                return targetPos;
            }
            set { targetPos = value; }
        }
        [SerializeField][HideInInspector]
        private Vector3 moveStartPosition;
        //单次移动开始的位置（最多和targetPos相差一格）?
        [HideInInspector]
        public Vector3 MoveStartPos { 
            get
            {
                if(!isVaildTargetPos)
                {
                    SetVaildTargetPos(this.Entity.Transform.Position);
                }
                return moveStartPosition;
            }
            set { moveStartPosition = value; }
        }

        /// <summary>
        /// 无效的移动目标, 为ture表示静止
        /// </summary>
        public bool InvaildTargetPos { get => MoveStartPos == TargetPos; }
        

        //速度相关
        private float extraSpeed;
        private float stepCostTime;
    
        public float StepCostTime { get => stepCostTime;}
        
        
        /// <summary>
        /// 表示是否切换idle动画 
        /// </summary>
        [HideInInspector]
        public bool IsIdling = true;
        public bool IsMoving { get => !InvaildTargetPos; }
        //Idle动画的子Id 每帧为设置为0
        [System.NonSerialized]
        public int IdleSubIndex = 0;
        /// <summary>
        /// 移动时的每秒能量消耗
        /// </summary>
        public float EnergyCostPerSecend = 10;
        public float EnergyNotEnoughSpeedParameter = 0.5f;
        public float FatigueTime = 0.1f;

        /// <summary>
        /// 记录过的目标地点
        /// </summary>
        [System.NonSerialized]
        public Vector3 RecordedTargetPos;

        public void SetVaildTargetPos(Vector3 currentPos)
        {
            currentPos = PositionUtil.CorrectPosition(currentPos);
            targetPos = currentPos;
            moveStartPosition = currentPos;
            isVaildTargetPos = true;
            ClearStepCostTime();
        }

    
        public void AddStepCostTime(float deltaTime)
        {
            stepCostTime += deltaTime;
        }

        public void ClearStepCostTime()
        {
            stepCostTime = 0;
        }
        
        public bool IsArrive(Vector3 pos)
        {
            return !IsMoving && Vector3.Distance(TargetPos, pos) < GameConstant.ZERO;
        }

        /// <summary>
        /// 回退到目标坐标(需要在TargetPos更新之前)
        /// </summary>
        public void BackToTargetPos()
        {
            Entity.Transform.Position = TargetPos;
        }

        // protected override void OnDisableRuntime()
        // {
        //     isVaildTargetPos = false;
        // }

        public void SetTargetInvaild()
        {
            isVaildTargetPos = false;
        }

        public void OnBeforeSave()
        {
            Speed.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            Speed.OnAfterLoad();
        }
    }

}
