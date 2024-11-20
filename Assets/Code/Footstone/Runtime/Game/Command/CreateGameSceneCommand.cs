namespace Lost.Runtime.Footstone.Game
{
    public class CreateGameSceneCommand : ICommand
    {
        public string NewScene;
        public bool IsDestoryCurrentScene;
        public GameSceneManager GameSceneManager;

        public void Execute()
        {
            GameSceneManager.CreateNewScene(NewScene, IsDestoryCurrentScene);
        }
    }

    
}
