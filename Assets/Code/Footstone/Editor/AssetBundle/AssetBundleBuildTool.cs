using UnityEditor;
using System.IO;
using UnityEngine;

namespace Lost.Editor.Footstone
{

    public class AssetBundleBuildWindow : SimpleWindow
    {
        
        [SerializeField]
        private string assetBundleDirectory;
        [SerializeField]
        private string assetDirectory;

        [MenuItem("Tools/Build Asset Bundle")]
        static void Open()
        {
            DisplayWizard<AssetBundleBuildWindow>("AssetBundleBuildWindow", "exit", "run");
        }

        static void BuildAllAssetBundles(string root)
        {
            
            if(Directory.Exists(root))
            {
                Directory.Delete(root, true);
            }
            Directory.CreateDirectory(root);
            BuildPipeline.BuildAssetBundles(root, 
                                            BuildAssetBundleOptions.None, 
                                            BuildTarget.StandaloneWindows);
        }


        static void CreateAssetBundleName(string root)
        {
            var absRoot = FileUtility.LocalPathToAbsPath(root);
            var directories = Directory.GetDirectories(absRoot, "*", SearchOption.AllDirectories);
            AssetDatabase.StartAssetEditing();
            foreach (var directory in directories)
            {
                var localPath = FileUtility.AbsPathToLocalPath(directory);
                
                var importer = AssetImporter.GetAtPath(localPath);
                bool isNeedReimpoart = false;
                if(importer.assetBundleName != localPath)
                {
                    importer.assetBundleName = localPath;
                    isNeedReimpoart = true;
                }
                if(importer.assetBundleVariant != null)
                {
                    importer.assetBundleVariant = null;
                    isNeedReimpoart = true;
                }
                
                if(isNeedReimpoart)
                    importer.SaveAndReimport();
                
            }
            AssetDatabase.StopAssetEditing();
        }
        

        private void OnWizardOtherButton() {
            BuildAllAssetBundles(assetBundleDirectory);
        }

    }

}

