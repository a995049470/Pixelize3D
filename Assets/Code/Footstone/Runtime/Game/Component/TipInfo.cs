using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class TipInfo
    {
        public ulong UID;
        public Vector3 Position;
        public float Timer;

        public TipInfo()
        {
            UID = 0;
            Position = Vector3.zero;
            Timer = 0;
        }

        public bool IsVaildTip()
        {
            return UID != 0 && Timer >= 0.3f;
        }
    }

}