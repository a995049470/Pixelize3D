using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Lost.Editor.Footstone
{
    public class ConvertEntitiesWindow : SimpleWindow
    {
        [SerializeField]
        private GameObject root;
        [Header("json默认使用root.name")]
        [SerializeField]
        private string jsonFloder;
        private Dictionary<string, (string, ResFlag)> _pathDic;
        private Dictionary<string, (string, ResFlag)> pathDic 
        {
            get
            {
                if(_pathDic == null)
                {
                    _pathDic = GetPathDic();
                }
                return _pathDic;
            }
        }

        [MenuItem("Tools/Convert Entities _F4")]
        static void Open()
        {
            var window = GetCurrentWindow<ConvertEntitiesWindow>();
            if(window != null) window.Close();
            else DisplayWizard<ConvertEntitiesWindow>("Convert Entities", "");
        }

        protected override bool DrawWizardGUI()
        {

            var res = base.DrawWizardGUI();

            if(GUILayout.Button("ToJson"))
            {
                ConvertEntityRootToJson();
            }

            return res;
        }

        private void ConvertEntityRootToJson()
        {
            if(root == null || string.IsNullOrEmpty(jsonFloder)) return;
            var datas = new List<EntityData>();
            var entities = root.GetComponentsInChildren<Entity>();
            foreach (var entity in entities)
            {
                if(TryConvertEntityToData(entity.gameObject, out var data))
                {
                    datas.Add(data);
                }
            }
            var json = LitJson.JsonMapper.ToJson(datas);
            var filePath = $"{jsonFloder}/{root.name}.json";
            if(!Directory.Exists(jsonFloder))
                Directory.CreateDirectory(jsonFloder);
            File.WriteAllText(filePath, json);
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath));
        }
        
        private Dictionary<string, (string, ResFlag)> GetPathDic()
        {
            var dic = new Dictionary<string, (string, ResFlag)>();
            ResPoolManager resPoolManager = EditorEnv.Service.GetService<ResPoolManager>();
            var flagNames = System.Enum.GetNames(typeof(ResFlag));
            foreach (var flagName in flagNames)
            {
                if(flagName.StartsWith("Entity_"))
                {
                    var flag = System.Enum.Parse<ResFlag>(flagName);
                    var config = resPoolManager.LoadResConfigDataNoCahe(flag);
                    foreach (var key in config.Keys)
                    {
                        var path = ((string)config[key][JsonKeys.url]);
                        if(dic.ContainsKey(path))
                        {
                            Debug.Log($"{path} has one more key");
                        }
                        dic[path] = (key, flag);
                    }
                }
            }
            return dic;
        }

        private bool TryConvertEntityToData(GameObject entityGo, out EntityData data)
        {
            bool isSuccess = false;
            data = null;
            string path = null;
            string log = "";
            if(entityGo != root.gameObject)
            {
                if(PrefabUtility.IsAnyPrefabInstanceRoot(entityGo))
                {
                    var source = PrefabUtility.GetCorrespondingObjectFromSource(entityGo);
                    path = AssetDatabase.GetAssetPath(source);
                }
                else
                {
                    log += "not prefab instance    ";
                }
            }

            if(!string.IsNullOrEmpty(path))
            {   
                if(pathDic.TryGetValue(path, out var reslut))
                {
                    data = EntityData.Create(reslut.Item1, reslut.Item2, entityGo);
                    isSuccess = true;
                }
                else
                {
                    log += "not in config";
                }
            }
            if(!string.IsNullOrEmpty(log))
            {
                log = $"{entityGo.name} : {log}";
                UnityEngine.Debug.Log(log);
            }
            return isSuccess;
        }

    }
}
