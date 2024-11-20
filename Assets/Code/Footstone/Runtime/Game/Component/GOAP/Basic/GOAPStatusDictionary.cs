using System.Collections.Generic;
using Lost.Runtime.Footstone.Collection;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GOAPStatusDictionary : SerializableDictionary<GOAPStatusFlag, GOAPStatus>
    {
        public void CopyTo(GOAPStatusDictionary src)
        {
            foreach (var kvp in this)
            {
                src[kvp.Key] = kvp.Value;
            }
        }

       

        public void Set(GOAPStatusFlag flag, bool val)
        {
            if(!this.TryGetValue(flag, out var status))
            {
                status = new GOAPStatus(flag);
                this[flag] = status;
            }
            status.Set(val);
        }
        

        public void Set(GOAPStatusFlag flag, float val, float scale)
        {
            if(!this.TryGetValue(flag, out var status))
            {
                status = new GOAPStatus(flag);
                this[flag] = status;
            }
            status.Set(val, scale);
        }

        public void Set(GOAPStatusFlag flag, int val)
        {
            if(!this.TryGetValue(flag, out var status))
            {
                status = new GOAPStatus(flag);
                this[flag] = status;
            }
            status.Set(val);
        }
        /// <summary>
        /// 强制获取GOAPStatus 若返回flase, 则表示返回的GOAPStatus是新建的
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ForceGetStatus(GOAPStatusFlag flag, out GOAPStatus status)
        {
            bool isNewStatus = false;
            if(!this.TryGetValue(flag, out status))
            {
                status = new GOAPStatus(flag);
                this[flag] = status;
                isNewStatus = true;
            }
            return isNewStatus;
        }

        public void ForceSetNewStatus(GOAPStatusFlag flag, bool val)
        {
            var isNew = ForceGetStatus(flag, out var status);
            if(isNew) status.Set(val);
        }

        public void ForceSetNewStatus(GOAPStatusFlag flag, int val)
        {
            var isNew = ForceGetStatus(flag, out var status);
            if(isNew) status.Set(val);
        }

        public void ForceSetNewStatus(GOAPStatusFlag flag, float val, float scale)
        {
            var isNew = ForceGetStatus(flag, out var status);
            if(isNew) status.Set(val, scale);
        }

        public bool GetBool(GOAPStatusFlag flag, bool defaultVal = false)
        {
            bool val = defaultVal;   
            if(this.TryGetValue(flag, out var status))
            {
                val = status.GetBool();
            }
            return val;
        }

        public float GetFloat(GOAPStatusFlag flag, float scale, float defaultVal = 0)
        {
            float val = defaultVal;   
            if(this.TryGetValue(flag, out var status))
            {
                val = status.GetFloat(scale);
            }
            return val;
        }

        public float GetInt(GOAPStatusFlag flag, int defaultVal = 0)
        {
            float val = defaultVal;   
            if(this.TryGetValue(flag, out var status))
            {
                val = status.GetInt();
            }
            return val;
        }
    }

}



