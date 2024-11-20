namespace Lost.Runtime.Footstone.Game
{
    public interface IComponentSaveOrLoadCallback
    {
        void OnBeforeSave();
        void OnAfterLoad();
    }

}



