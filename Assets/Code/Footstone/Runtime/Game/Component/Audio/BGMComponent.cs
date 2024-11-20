using Lost.Runtime.Footstone.Collection;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    [DefaultEntityComponentProcessor(typeof(BGMProcessor))]
    [DefaultEntityComponentProcessor(typeof(BGMPlayProcessor))]
    public class BGMComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        [UnityEngine.SerializeField][UnityEngine.HideInInspector]
        private SerializableStack<string> bgmStack = new();

        public string GetCurrentBGM()
        {
            string current = null;
            if(bgmStack.Count > 0) current = bgmStack.Peek();
            return current;
        }

        public void SwitchBGM(string key)
        {
            if(bgmStack.Count > 0) bgmStack.Pop();
            bgmStack.Push(key);
        }

        public void PushBGM(string key)
        {
            bgmStack.Push(key);
        }

        public void PopBGM()
        {
            if(bgmStack.Count > 0) bgmStack.Pop();
        }


        public void OnBeforeSave()
        {
            bgmStack.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            bgmStack.OnAfterLoad();
        }
    }


}