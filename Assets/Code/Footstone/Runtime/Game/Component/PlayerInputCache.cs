using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class PlayerInputCache
    {
        public PlayerInputCache(float late)
        {
            maxLate = late;
        } 

        private Stack<(float, KeyCode)> cacheInputs = new(8);
        private float maxLate;
        
        public void CacheInput(float time, KeyCode keyCode)
        {
            cacheInputs.Push((time, keyCode));
        }

        public bool TryPeekInput(float time, out KeyCode code)
        {
            bool isGet = false;
            code = KeyCode.None;
            if(cacheInputs.Count > 0)
            {
                var item = cacheInputs.Peek();
                if(time <= item.Item1 + maxLate)
                {
                    code = item.Item2;
                    isGet = true;
                }
                else
                    cacheInputs.Clear();
            }
            return isGet;
        }

        public bool TryGetInput(float time, out KeyCode code)
        {
            bool isGet = false;
            code = KeyCode.None;
            if(cacheInputs.Count > 0)
            {
                var item = cacheInputs.Pop();
                if(time <= item.Item1 + maxLate)
                {
                    code = item.Item2;
                    isGet = true;
                }
                else
                    cacheInputs.Clear();
            }
            return isGet;
        }

        public void CostInput()
        {
            cacheInputs.Pop();
        }
    }

}
