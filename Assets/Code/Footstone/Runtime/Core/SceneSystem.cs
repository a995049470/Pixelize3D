using System;

namespace Lost.Runtime.Footstone.Core
{

    public class SceneSystem : IUpdateable
    {
        public SceneInstance SceneInstance { get; private set; }

        public bool Enabled => true;

        public int UpdateOrder => 0;

        public IServiceRegistry Services { get; private set; }

        public SceneSystem(IServiceRegistry service)
        {
            Services = service;
            SceneInstance = new(service, new Scene());
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public void Update(GameTime gameTime)
        {
            SceneInstance.Update(gameTime);
        }
    }
}



