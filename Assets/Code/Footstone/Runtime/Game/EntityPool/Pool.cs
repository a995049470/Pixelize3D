using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public abstract class Pool<T> 
    {
        private Stack<T> entityStack;
        private int capacity;
        public Pool(int _capacity)
        {
            capacity = _capacity;
            entityStack = new Stack<T>(_capacity);
        }

        protected virtual void OnPop(T t)
        {
            
        } 

        protected virtual void OnPush(T t)
        {

        }

        protected virtual void OnDestory(T t)
        {

        }


        public bool TryGet(out T t)
        {
            bool isPop = entityStack.TryPop(out t);
            if(isPop) OnPop(t);
                
            return isPop;
        }

        public void SetCapacity(int _capacity)
        {
            if(_capacity >= 0 && capacity != _capacity)
            {
                capacity = _capacity;
                while (entityStack.Count > capacity)
                {
                    OnDestory(entityStack.Pop());
                }
            }
        }

        public void Recycle(T t)
        {
            if(entityStack.Count == capacity)
                OnDestory(t);
            else
            {
                OnPush(t);
                entityStack.Push(t);
            }
        }

        public void DestoryAll()
        {
            while (entityStack.Count > 0)
            {
                OnDestory(entityStack.Pop());
            }
        }
    }
}