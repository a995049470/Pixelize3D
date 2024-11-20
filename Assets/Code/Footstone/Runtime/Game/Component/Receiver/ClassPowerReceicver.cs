using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class ClassPowerReceicver<T> where T : class
    {
        protected Dictionary<ulong, T> powerDic = new Dictionary<ulong, T>();
        [SerializeField][HideInInspector]
        private List<ulong> cache0 = new();
        [SerializeField][HideInInspector]
        private List<T> cache1 = new();
        [SerializeField][HideInInspector]
        private ulong dataUID = 0;
        private T data;
        public T Data { get => data; }

        public void ReceivePower(ulong uid, T value)
        {
            powerDic.Add(uid, value);
            if(data == null) 
            {
                data = value;
                dataUID = uid;
            }
        }

        public bool LostPower(ulong uid)
        {
            bool isRemove = false;
            if(powerDic.Remove(uid))
            {
                if(powerDic.Count == 0)
                {
                    isRemove = true;
                    data = null;
                }
                else if(uid == dataUID)
                {
                    data = powerDic.Values.GetEnumerator().Current;
                }
            }
            return isRemove;
        }

       
        public void OnBeforeSave()
        {
            cache0.Clear();
            cache1.Clear();
            cache0.AddRange(powerDic.Keys);
            cache1.AddRange(powerDic.Values);
        }

        public void OnAfterLoad()
        {
            powerDic.Clear();
            var count = cache0.Count;
            for (int i = 0; i < count; i++)
            {
                powerDic[cache0[i]] = cache1[i];
            }
            powerDic.TryGetValue(dataUID, out data);
            cache0.Clear();
            cache1.Clear();
        }
    }

}



