using Lost.Runtime.Footstone.Core;
using Unity.Mathematics;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

   
    [DisallowMultipleComponent]
    [DefaultEntityComponentProcessor(typeof(RotateStateProcessor))]
    [DefaultEntityComponentProcessor(typeof(RotateProcessor))]
    public class RotateComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        public bool IsRandomDirection = false;

        [SerializeField]
        public CharacterAttribute RotateSpeed = new();
        [HideInInspector]
        public Vector3 FaceDirection;
        
        public float GetRotateSpeedRad(int frame)
        {
            return RotateSpeed.GetFinalValue(frame) * Mathf.PI;
        }

        public void Rotate(float angle)
        {
            if(FaceDirection != Vector3.zero)
            {
                FaceDirection = Quaternion.AngleAxis(angle, Vector3.up) * FaceDirection;
                for (int i = 0; i < 3; i++)
                {
                    FaceDirection[i] = Mathf.RoundToInt(FaceDirection[i]);
                }
            }
        }

        public bool IsRotating()
        {
            return !DirectionUtil.IsCorrectFaceDir(FaceDirection, this.Entity.Transform.Forward);
        }

        public void OnBeforeSave()
        {
            RotateSpeed.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            RotateSpeed.OnAfterLoad();
        }
    }

}
