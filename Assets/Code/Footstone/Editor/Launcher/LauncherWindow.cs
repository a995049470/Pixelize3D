using System.Collections;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Game;
using UnityEditor;
using UnityEngine;

namespace Lost.Editor.Footstone
{

    public class LauncherWindow : SimpleWindow
    {
        [SerializeField]
        private string defalutScene;
        [SerializeField]
        private bool isUseDefalutScene;
        
        
        [MenuItem("Tools/Launcher Setting _F1")]
        static void Open()
        {
            var window = GetCurrentWindow<LauncherWindow>();
            if(window != null) window.Close();
            else DisplayWizard<LauncherWindow>("Launcher Setting", "");
        }

        protected override void Save()
        {
            base.Save();
            if(isUseDefalutScene)
                EditorPrefs.SetString(GameConstant.DefalutSceneKey, defalutScene);
            else
                EditorPrefs.DeleteKey(GameConstant.DefalutSceneKey);
        }

    }
}
