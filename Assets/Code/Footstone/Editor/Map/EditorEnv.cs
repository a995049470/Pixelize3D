using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;

namespace Lost.Editor.Footstone
{
    public class EditorEnv
    {
        private static IServiceRegistry service;
        public static IServiceRegistry Service{
            get{
                if(service == null) 
                    service = GetEditorEnv();
                return service;
            }
        }

        
        private static IServiceRegistry GetEditorEnv()
        {
            var service = new ServiceRegistry();
            LitJsonUtil.LitJsonRegister();
            service.AddService(new ContentManager());
            service.AddService(new JsonDataManager(service));
            service.AddService(new ResPoolManager(service));
            return service;
        }
    }

}
