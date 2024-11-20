using System.Collections.Generic;
using LitJson;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Lost.Editor.Footstone
{
    public class GMWindow : SimpleWindow
    {
        [SerializeField][Range(1, 8)]
        private int rowItemCount = 1;
        [SerializeField]
        private string itemFilter;
        [SerializeField]
        private int itemCount;
        private List<string> itemKeys;
        
        private string cacheFilter = null;
        private List<string> activeItemKeys = new();
        [System.NonSerialized]
        public GameArchive Archive = new();
        [SerializeField][HideInInspector]
        private string windowKey;

        [SerializeField][HideInInspector]
        private Vector3 targetPos;
        [SerializeField][HideInInspector]
        private int day;
        [SerializeField][HideInInspector]
        private bool isItemFlodout = true;
        [SerializeField][HideInInspector]
        private string knownKey;

        private List<string> GetItemKeys()
        {
            var resPoolManager = GetServiceRuntime<ResPoolManager>();
            var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item);
            var keys = new List<string>(data.Keys);
            return keys;
        }


        [MenuItem("Tools/GMWindow _F3")]
        static void Open()
        {
            var window = GetCurrentWindow<GMWindow>();
            if(window != null) window.Close();
            else DisplayWizard<GMWindow>("GMWindow", "");
        }

        private bool IsPass(string key, string fliter)
        {
            var len1 = key?.Length ?? 0;
            var len2 = fliter?.Length ?? 0;
            var ptr2 = 0;
            for (int ptr1 = 0; ptr1 < len1; ptr1++)
            {
                if(ptr2 == len2) break;
                if(key[ptr1] == fliter[ptr2])
                    ptr2 ++;
            }
            bool isPass = ptr2 == len2;
            if(!isPass)
            {
                var itemData = GetItemData(key);
                var itemName = (string)itemData[JsonKeys.name];
                isPass |= itemName.Contains(fliter); 
            }
            return isPass;
        }

        private void RefreshData()
        {
            if ((itemKeys?.Count ?? 0) == 0)
            {
                itemKeys = GetItemKeys();
                cacheFilter = null;
            }
            if (cacheFilter != itemFilter)
            {
                cacheFilter = itemFilter;
                activeItemKeys.Clear();
                itemKeys.ForEach(key =>
                {
                    if (!string.IsNullOrEmpty(key) && IsPass(key, itemFilter))
                    {
                        activeItemKeys.Add(key);
                    }
                });
            }
        }

        private void ClearOnEditor()
        {
            cacheFilter = null;
        }

        private BagData GetBagDataRuntime()
        {
            return GetServiceRuntime<SceneSystem>().SceneInstance.GetProcessor<BagProcessor>().BagComp.Data;
        }

        private JsonData GetItemData(string key)
        {
            var resPoolManager = GetServiceRuntime<ResPoolManager>();
            var data = resPoolManager.LoadResConfigData(ResFlag.Config_Item);
            return data[key];
        }

        private void ShowItem()
        {

            isItemFlodout = EditorGUILayout.Foldout(isItemFlodout, "所有物品");
            if(isItemFlodout)
            {
                RefreshData();
                for (int i = 0; i < activeItemKeys.Count; i++)
                {
                    if (i % rowItemCount == 0)
                        EditorGUILayout.BeginHorizontal();
                    var key = activeItemKeys[i];
                    var width = this.position.width / rowItemCount - 5f;
                    var itemData = GetItemData(key);
                    var itemName = (string)itemData[JsonKeys.name];
                    if (GUILayout.Button(itemName, GUILayout.Width(width)))
                    {
                        var bagData = GetBagDataRuntime();
                        var itemMaxCount = (int)itemData[JsonKeys.maxNum];
                        var currentCount = Mathf.Min(itemMaxCount, Mathf.Abs(itemCount));
                        if (itemCount > 0)
                            bagData.ReceiveItem(key, currentCount);
                        else if (itemCount < 0)
                            bagData.LoseItem(key, currentCount);
                    }
                    if (i % rowItemCount == rowItemCount - 1)
                        EditorGUILayout.EndHorizontal();
                }

                if (activeItemKeys.Count % rowItemCount != 0)
                    EditorGUILayout.EndHorizontal();
            }
        }

        private void SaveLoad()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存"))
            {
                var storageSystem = StoneHeart.Instance.Services.GetService<StorageSystem>();
                storageSystem.Save();
            }

            if (GUILayout.Button("读档"))
            {
                var storageSystem = StoneHeart.Instance.Services.GetService<StorageSystem>();
                storageSystem.Load();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("打开界面："))
            {
                GetServiceRuntime<UIManager>().OpenWindow(windowKey);
            }
            windowKey = EditorGUILayout.TextField(windowKey);
            EditorGUILayout.EndHorizontal();
        }

        private void ShowTransport()
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("传送"))
            {
                var PlayerProcessor = GetServiceRuntime<SceneSystem>().SceneInstance.GetProcessor<PlayerProcessor>();
                var playerEntity = PlayerProcessor?.Target?.Entity;
                if(playerEntity)
                {
                    var comp = playerEntity.GetOrCreate<TransportComponent>();
                    comp.TargetPosition = targetPos;
                }
            }
            targetPos = EditorGUILayout.Vector3Field("", targetPos);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();


            var newSceneCollectionProcessor = GetProcessorRuntime<NewSceneCollectionProcessor>();
            var datas = newSceneCollectionProcessor.ComponentDatas;
            foreach (var kvp in datas)
            {
                var newSceneComp = kvp.Key;
                if(GUILayout.Button(newSceneComp.Scene))
                {
                    var PlayerProcessor = GetProcessorRuntime<PlayerProcessor>();
                    var playerEntity = PlayerProcessor?.Target?.Entity;
                    if(playerEntity)
                    {
                        var pos = newSceneComp.Entity.Transform.Position;
                        var comp = playerEntity.GetOrCreate<TransportComponent>();
                        comp.TargetPosition = pos;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ShowTime()
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("时传(天)"))
            {
                var timeProcessor = GetServiceRuntime<SceneSystem>().SceneInstance.ForceGetProcessor<TimeProcessor>();
                var timeComp = timeProcessor.SingleComponent;
                if(timeComp)
                {
                    timeComp.TotalGrowthPower += Mathf.Max(0, day);
                    timeComp.Day += Mathf.Max(0, day);
                }
                
            }
            day = EditorGUILayout.IntField("", day);
            EditorGUILayout.EndHorizontal();
        }

        private void ShowConfig()
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("重新加载Json"))
            {
                GetServiceRuntime<ResPoolManager>().ClearConfigDataCache();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowFoundLevel()
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("发现"))
            {
                var mapProcessor = GetServiceRuntime<SceneSystem>().SceneInstance.ForceGetProcessor<MapProcessor>();
                var mapComp = mapProcessor.SingleComponent;
                if(mapComp && !string.IsNullOrEmpty(knownKey))
                {
                    mapComp.Data.AddKnownKey(knownKey);
                }
            }
            knownKey = EditorGUILayout.TextField("", knownKey);
            EditorGUILayout.EndHorizontal();
        }

        protected override bool DrawWizardGUI()
        {
            var res = base.DrawWizardGUI();
            if(Application.isPlaying)
            {
                
                ShowItem();
                SaveLoad();
                ShowTransport();
                ShowTime();
                ShowUI();
                ShowFoundLevel();
                ShowConfig();
            }
            else
                ClearOnEditor();
            
            return res;
        }
    }
}

