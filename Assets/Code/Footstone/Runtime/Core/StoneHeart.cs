using UnityEngine;
namespace Lost.Runtime.Footstone.Core
{

    public class StoneHeart
    {
        public static StoneHeart Instance { get; private set; }
        public IServiceRegistry Services{ get ; private set; }
        private StoneHeartDance dance;
        

        public StoneHeart()
        {
            var go = new GameObject("StoneHeartDance");
            dance = go.AddComponent<StoneHeartDance>();
            Services = new ServiceRegistry();
            Services.ServiceAdded += OnServiceAdded;
            Services.ServiceRemoved += OnServiceRemoved;
            Services.AddService(new InputManager());
            Services.AddService(new SceneSystem(Services));
            Services.AddService(new ContentManager());
            Services.AddService(new PhysicsSystem());
            Services.AddService(new UniqueIdManager());
           
            Services.AddService(dance);
            
        }

        private void OnServiceAdded(object o, ServiceEventArgs e)
        {
            if(e.Instance is IUpdateable updateable)
            {
                dance.UpdateableCollection.Add(updateable);
            }
        }

        private void OnServiceRemoved(object o, ServiceEventArgs e)
        {
             if(e.Instance is IUpdateable updateable)
            {
                dance.UpdateableCollection.Remove(updateable);
            }
        }

       
        public static void OnGameStart()
        {
            Instance = Instance ?? new StoneHeart();
        }
    }
}



