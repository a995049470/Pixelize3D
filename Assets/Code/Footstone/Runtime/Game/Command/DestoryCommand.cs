using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class DestoryCommand : ICommand
    {
        public Entity Target;
        public void Execute()
        {
            Target.DestoryUnityGameObject();
            Target = null;
        }
    }
}
