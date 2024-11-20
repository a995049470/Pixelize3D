using Lost.Runtime.Footstone.Core;
using System.Threading;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    public class LevelManager : IManager
    {
        private IServiceRegistry service;
        public LevelManager(IServiceRegistry _service)
        {
            service = _service;
        }

       
        
        public void LoadLevel(string key, Vector2Int origin)
        {
            var loader = new LevelLoader(service, origin, key);
            loader.LoadLevel();
        }



    }
}
