using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public static class DirectionUtil
    {
        //10°的cos值
        private const float cos10 = 0.9848f;
        public static bool IsCorrectFaceDir(Vector3 faceDir, Vector3 targetDir)
        {
            return Vector3.Dot(faceDir, targetDir) > cos10;
        }
        public static Vector3 CalculateDirection(Vector3 dir)
        {
            dir.y = 0; 
            if(Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) dir.z = 0;
            else dir.x = 0;
            dir.Normalize();
            return dir;
        }
        public static Vector3 CalculateDirection(Vector3 dir, Vector3 defalutDir)
        {
            if(dir == Vector3.zero)
            {
                dir = defalutDir;
            }
            else
            {
                dir.y = 0; 
                if(Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) dir.z = 0;
                else dir.x = 0;
                dir.Normalize();
            }
            return dir;
        }
    }
}

