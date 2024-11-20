using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    [System.Serializable]
    public class PowerReceicver
    {
        private HashSet<ulong> powerUIDSet = new HashSet<ulong>();
        [SerializeField][HideInInspector]
        private List<ulong> cache0 = new();

        public void ReceivePower(ulong uid)
        {
            powerUIDSet.Add(uid);
        }

        public bool LostPower(ulong uid)
        {
            bool isRemove = false;
            if(powerUIDSet.Remove(uid) && powerUIDSet.Count == 0)
            {
                isRemove = true;
            }
            return isRemove;
        }

       
        public void OnBeforeSave()
        {
            cache0.Clear();
            cache0.AddRange(powerUIDSet);
        }

        public void OnAfterLoad()
        {
            powerUIDSet.Clear();
            cache0.ForEach(x => powerUIDSet.Add(x));
            cache0.Clear();
        }

        
    }

}



