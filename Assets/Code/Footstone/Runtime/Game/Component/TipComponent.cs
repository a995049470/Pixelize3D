using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    //UI上的提示标签
    [DefaultEntityComponentProcessor(typeof(TipProcessor))]
    public class TipComponent : EntityComponent
    {
        private TipInfo interactiveTipInfo;
        public TipInfo InteractiveTipInfo { get => interactiveTipInfo;}

        protected override void Awake()
        {
            base.Awake();
            interactiveTipInfo = new();
        }

        public void UpdateInteractiveTipInfo(ulong uid, Vector3 pos, float deltaTime)
        {
            interactiveTipInfo.UID = uid;
            if(interactiveTipInfo.Position == pos)
            {
                interactiveTipInfo.Timer += deltaTime;
            }
            else
            {
                interactiveTipInfo.Position = pos;
                interactiveTipInfo.Timer = 0;
            }
        }

        public void ClearInteractiveTipInfo(ulong uid)
        {
            if(interactiveTipInfo.UID == uid)
            {
                interactiveTipInfo.UID = 0;
                interactiveTipInfo.Timer = 0;
            }
        }


    }

}