using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.Rendering.Universal
{
    public class RenderPassSharedData
    {
        private static RenderPassSharedData instance;
        public static RenderPassSharedData Instance
        {
            get{
                if(instance == null) instance = new RenderPassSharedData();
                return instance;
            }
        }

        private Dictionary<int, Vector4> dic = new Dictionary<int, Vector4>();
        
        public void SetInt(int id, int val)
        {
            dic[id] = new Vector4(val, 0, 0, 0);
        }
        public void SetFloat(int id, float val)
        {
            dic[id] = new Vector4(val, 0, 0, 0);
        }
        
        public void SetVector(int id, Vector4 val)
        {
            dic[id] = val;
        }

        public int GetInt(int id, int defaultValue = 0)
        {
            var res = defaultValue;
            if(dic.TryGetValue(id, out var v))
            {
                res = (int)v.x;
            }
            return res;
        }

        public float GetFloat(int id, float defaultValue = 0)
        {
            var res = defaultValue;
            if(dic.TryGetValue(id, out var v))
            {
                res = v.x;
            }
            return res;
        }
        public Vector4 GetVector(int id, Vector4 defaultValue)
        {
            var res = defaultValue;
            if(dic.TryGetValue(id, out var v))
            {
                res = v;
            }
            return res;
        }

    }
}

