using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public static class AttackUtil
    {
        public static bool IsCorrectFaceDir(Vector3 faceDir, Vector3 attackDir)
        {
            return DirectionUtil.IsCorrectFaceDir(faceDir, attackDir);
        }

        public static bool Attackable(AttackComponent attackComp, ActionMaskComponent actionMaskComp)
        {
            return !attackComp.IsAttacking && actionMaskComp.IsActionEnable(ActionFlag.Attack);
        }

        public static void StartAttackNoCheck(AttackComponent attackComp, ActionMaskComponent actionMaskComp, RotateComponent rotateComp, Vector3 attackDir, int attackIndex)
        {

            attackComp.ApplyAttackCommand(attackDir, attackIndex);
            rotateComp.FaceDirection = attackDir;
            if(attackComp.IsBanActionOnAttaking)
            {
                actionMaskComp.DisabledAction(ActionFlag.Move);
            }

        }

        public static Vector3 CacluteAttackDir(Vector3 origin, Vector3 target)
        {
            var dir = target - origin;
            dir.y = 0;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) dir.z = 0;
            else dir.x = 0;
            if (dir == Vector3.zero) dir.x = 1;
            dir.Normalize();
            return dir;
        }

    }
}

