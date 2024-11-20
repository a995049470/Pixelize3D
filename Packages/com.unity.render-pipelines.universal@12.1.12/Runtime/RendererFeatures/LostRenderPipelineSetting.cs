using System.Collections.Generic;

#if UNITY_EDITOR
#endif

namespace UnityEngine.Rendering.Universal
{
    public static class LostRenderPipelineSetting
    {
        private static Dictionary<string, Matrix4x4> matrixDic = new();
        private static Dictionary<string, Vector4> vectorDic = new();

        public static void Set(string key, Matrix4x4 value)
        {
            matrixDic[key] = value;
        }

        public static void Set(string key, Vector4 value)
        {
            vectorDic[key] = value;
        }

        public static void Set(string key, float value)
        {
            vectorDic[key] = new Vector4(value, 0);
        }

        public static void Set(string key, int value)
        {
            vectorDic[key] = new Vector4(value, 0);
        }

        public static bool TryGet(string key, out Matrix4x4 value)
        {
            return matrixDic.TryGetValue(key, out value);
        }

        public static bool TryGet(string key, out Vector4 value)
        {
            return vectorDic.TryGetValue(key, out value);
        }

        public static bool TryGet(string key, out float value)
        {
            var isGet = vectorDic.TryGetValue(key, out var v4);
            value = v4.x;
            return isGet;
        }

        public static bool TryGet(string key, out int value)
        {
            var isGet = vectorDic.TryGetValue(key, out var v4);
            value = (int)v4.x;
            return isGet;
        }
        
        public static void Clear()
        {
            matrixDic.Clear();
            vectorDic.Clear();
        }
    }
}
