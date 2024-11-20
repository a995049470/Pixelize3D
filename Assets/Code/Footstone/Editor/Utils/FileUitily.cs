using System.Collections;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

namespace Lost.Editor.Footstone
{
    public class FileUtility 
    {
        public static string LocalPathToAbsPath(string path)
        {
            if(path == null || !path.StartsWith("Assets/"))
            {
                return null;
            }
            var absPath = Application.dataPath + path.Substring(6);
            absPath = absPath.Replace('/','\\');
            return absPath;
        }

        public static string AbsPathToLocalPath(string path)
        {
            var len = Application.dataPath.Length - 6;
            
            return path.Substring(len);
        }

        public static string GetNearestFloder(string path)
        {
            var sp = path.Split('/', '\\');
            var ex = sp.Length > 1 ? 1 : 0;
            var len = path.Length - sp[sp.Length - 1].Length - ex;
            return path.Substring(0, len);
        }

        public static List<T> LoadObjectsFromFloder<T>(string localFloder, string searchPattern) where T : Object
        {
           var absFiles =  Directory.GetFiles(LocalPathToAbsPath(localFloder), searchPattern, SearchOption.AllDirectories);
           var len = absFiles.Length;
           var res = new List<T>(len);
           foreach (var absFile in absFiles)
           {
                var localFile = AbsPathToLocalPath(absFile);
                var asset = AssetDatabase.LoadAssetAtPath<T>(localFile);
                if(asset != null) res.Add(asset); 
           }

           return res;
        }

        
    }
}

