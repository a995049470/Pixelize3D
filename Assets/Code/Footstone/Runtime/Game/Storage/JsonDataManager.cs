using System.Collections.Generic;
using LitJson;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    
    
    public class JsonDataManager
    {
        //private IServiceRegistry service;
        private ContentManager content;

        public JsonDataManager(IServiceRegistry service)
        {
            content = service.GetService<ContentManager>();
        }
       
    }
}



