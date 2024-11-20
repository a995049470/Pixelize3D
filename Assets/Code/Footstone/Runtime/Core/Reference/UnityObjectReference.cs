using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    [System.Serializable]
    public abstract class UnityObjectReference 
    {
    #region 编辑器相关
        public abstract bool UpdateUnityObject(Object o);
        public abstract Object GetDisplayObject();
        protected abstract void UpdateReference();  
        public abstract System.Type GetObjectType(); 
        public abstract Object GetOwner();
    #endregion
    }

}
