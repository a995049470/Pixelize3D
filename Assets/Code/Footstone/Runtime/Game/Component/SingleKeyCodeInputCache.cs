using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Game
{
    public class SingleKeyCodeInputCache
    {
        private Stack<float> cacheInputs = new(8);
        private float maxLate;
        public SingleKeyCodeInputCache(float late)
        {
            maxLate = late;
        } 
        public void CacheInput(float time)
        {
            cacheInputs.Push(time);
        }

        public bool TryGetInput(float time)
        {
            bool isGet = false;
            if(cacheInputs.Count > 0)
            {
                var item = cacheInputs.Pop();
                if(time <= item + maxLate)
                {
                    isGet = true;
                }
                else
                    cacheInputs.Clear();
            }
            return isGet;
        }
    }

}
