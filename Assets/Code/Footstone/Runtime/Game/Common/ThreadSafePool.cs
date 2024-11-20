using System;
using System.Collections.Concurrent;

namespace Lost.Runtime.Footstone.Game
{
    public class ThreadSafePool<T> where T : class, new()
    {
        private ConcurrentBag<T> bag = new ConcurrentBag<T>();
        private Func<T> func;
        
        public ThreadSafePool(Func<T> f = null)
        {
            func = f;
        }

        public T Take()
        {
            if(!bag.TryTake(out var res))
            {
                if (func != null) res = func.Invoke();
                else res = new T();
            }
            return res;
        }

        public void Add(T t)
        {
            bag.Add(t);
        }
    }
}