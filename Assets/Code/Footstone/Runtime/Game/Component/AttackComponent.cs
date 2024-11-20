using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(AttackStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(AttackUpdateActionMaskProcessor))]
    [DefaultEntityComponentProcessor(typeof(AttackProcessor))]
    [DefaultEntityComponentProcessor(typeof(DeadUpdateActionMaskProcessor))]
    public class AttackComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [SerializeField]
        private AttackDataTableReference data = new();
        
        [HideInInspector]
        public float AttackCostTime;
        [HideInInspector]
        public int AttackEventPtr;

        [HideInInspector]
        public Vector3 AttackDir;
        [HideInInspector]
        public bool IsAttacking;
        [HideInInspector]
        public int AttackIndex = 0;

        [System.NonSerialized]
        public bool IsCorrectFaceDir;
        private bool isNeedCostEnergy = false;
        public bool HasCostEnery { get => !isNeedCostEnergy; }
        
        [HideInInspector]
        public AttackDataTableReceicver AttackDataReceicver = new();
    
        public AttackData Data { get => GetCurrentAttackData(); }


        public CharacterAttribute AttackSpeed = new(1.0f);
        public CharacterAttribute AttackAttribute = new();
        public bool IsBanActionOnAttaking { get => Data.IsBanAction; }

        [HideInInspector]
        public bool IsPlayAttack;
        
        public void ApplyAttackCommand(Vector3 dir, int index = 0)
        {
            AttackDir = dir;
            AttackCostTime = 0;
            IsAttacking = true;
            AttackEventPtr = 0;
            IsCorrectFaceDir = false;
            isNeedCostEnergy = true;
            AttackIndex = index;
        }        

        public void AttackCombo(int index)
        {
            AttackCostTime = 0;
            IsAttacking = true;
            AttackEventPtr = 0;
            IsCorrectFaceDir = true;
            isNeedCostEnergy = true;
            AttackIndex = index;
        }

        public AttackData GetCurrentAttackData()
        {
            var table = AttackDataReceicver.Data?.Asset ?? data.Asset;
            return table.GetAttackData(AttackIndex);
        }

        public bool TryStartCostEnergy()
        {
            return isNeedCostEnergy && IsAttacking;
        }

        public void EndCostEnery()
        {
            isNeedCostEnergy = false;
        }

        public bool TryCompleteAttack(float deltaTime)
        {
            AttackCostTime += deltaTime;
            bool isComplete = AttackCostTime >= Data.AttackDuration;
            if(isComplete)
            {
                IsAttacking = false;
            }
            return isComplete;
        }

        public void ForceCompleteAttack()
        {
            IsAttacking = false;
            isNeedCostEnergy = false;
            IsCorrectFaceDir = false;
        }

        public void OnBeforeSave()
        {
            AttackDataReceicver.OnBeforeSave();
            AttackAttribute.OnBeforeSave();
            AttackSpeed.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            AttackDataReceicver.OnAfterLoad();
            AttackAttribute.OnAfterLoad();
            AttackSpeed.OnAfterLoad();
        }

        
    }
}
