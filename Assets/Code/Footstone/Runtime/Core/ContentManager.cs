using UnityEngine;
namespace Lost.Runtime.Footstone.Core
{
    
    public class ContentManager
    {
        private string ConvertUrl(string url)
        {
            var id = url.LastIndexOf('.');
            return url.Substring(11, id - 11);
        }

        //TODO:考虑资源的释放....
        public T LoadRes<T>(string url) where T : Object
        {
            url = ConvertUrl(url);
            var asset = Resources.Load<T>(url);
            if(asset == null) 
            {
                Debug.LogError($"{url} Invaild Path.....");
            }
            return asset;
        }
        
        public Entity LoadEntity(string url)
        {
            return LoadComponent<Entity>(url);
        }


        public T LoadComponent<T>(string url) where T : MonoBehaviour
        {
            var go = LoadRes<GameObject>(url);
            var t = go.GetComponent<T>();
            if(t == null) Debug.LogError($"{url} No {typeof(T)} Componet.....");
            return t;
        }
    }
}



