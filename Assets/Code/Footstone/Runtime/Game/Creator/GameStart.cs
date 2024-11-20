
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using UnityEngine;


namespace Lost.Runtime.Footstone.Game
{

    // [System.Serializable]
    // public struct EntityTag 
    // {
    //     public string Key;
    //     public ResFlag Flag;
    // }


    public class GameStart : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        private void Awake()
        {
            GameLauncher.GameStart();
            var uiManager = StoneHeart.Instance.Services.GetService<UIManager>();
            uiManager.ClearInvaildWindow();
            var gameSceneManager = StoneHeart.Instance.Services.GetService<GameSceneManager>();
            gameSceneManager.CreateNewScene(sceneName, true);
            GameObject.DestroyImmediate(this.gameObject);
        }

    }
    
}
