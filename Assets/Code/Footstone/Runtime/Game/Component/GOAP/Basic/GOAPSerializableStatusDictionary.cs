using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GOAPSerializableStatusDictionary
    {
        [SerializeField]
        private List<GOAPStatus> cacheData = new();
        private bool isDitry = true;
        private GOAPStatusDictionary _dic = new();
        public GOAPStatusDictionary Dictionary
        {
            get
            {
                if(isDitry)
                {
                    isDitry = false;
                    _dic.Clear();
                    cacheData.ForEach(x => _dic[x.Flag] = x);
                }
                return _dic;
            }
        }

        public GOAPSerializableStatusDictionary()
        {
            isDitry = true;
        }

        public void SetDirty()
        {
            isDitry = true;
        }
    }

}



