using LitJson;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public static class LitJsonUtil
    {
        public static Vector3Int ToVector3Int(JsonData jsonData)
        {
            var x = (int)jsonData[0];
            var y = (int)jsonData[1];
            var z = (int)jsonData[2];
            return new Vector3Int(x, y, z);
        }

        public static int[] ToIntArray(JsonData jsonData)
        {
            var count = jsonData.Count;
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = ((int)jsonData[i]);
            }
            return result;
        }        

        public static float[] ToFloatArray(JsonData jsonData)
        {
            var count = jsonData.Count;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = ((float)jsonData[i]);
            }
            return result;
        }      

        public static string[] ToStringArray(JsonData jsonData)
        {
            var count = jsonData.Count;
            string[] result = new string[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = ((string)jsonData[i]);
            }
            return result;
        }      

        public static void LitJsonRegister()
        {
            LitJson.JsonMapper.RegisterExporter<Vector3>((v3, writer) =>
            {
                writer.WriteObjectStart();
                writer.WritePropertyName("x");
                writer.Write(v3.x);
                writer.WritePropertyName("y");
                writer.Write(v3.y);
                writer.WritePropertyName("z");
                writer.Write(v3.z);
                writer.WriteObjectEnd();
            });

            LitJson.JsonMapper.RegisterExporter<Quaternion>((q, writer) =>
            {
                writer.WriteObjectStart();
                writer.WritePropertyName("x");
                writer.Write(q.x);
                writer.WritePropertyName("y");
                writer.Write(q.y);
                writer.WritePropertyName("z");
                writer.Write(q.z);
                writer.WritePropertyName("w");
                writer.Write(q.w);
                writer.WriteObjectEnd();
            });

            LitJson.JsonMapper.RegisterImporter<long, ulong>(input => (ulong)input);
        }

    }
}
