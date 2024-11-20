using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public static class PositionUtil
    {
        public static Vector3 CorrectPosition(Vector3 pos)
        {
            pos.x = Mathf.Round(pos.x);
            pos.y = 0;
            pos.z = Mathf.Round(pos.z);
            return pos;
        }

        public static bool IsEqual(Vector3 left, Vector3 right)
        {
            return left == right;
        }
    }
}

