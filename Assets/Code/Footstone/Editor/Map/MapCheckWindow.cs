using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lost.Runtime.Footstone.Core;
using Lost.Runtime.Footstone.Game;
using System;
using UnityEditor;
using LitJson;

namespace Lost.Editor.Footstone
{

    public class MapCheckWindow : SimpleWindow
    {
        [SerializeField]
        private string mapkey;
        [SerializeField]
        private Vector2Int seedRange;

        [MenuItem("Tools/Map Check Window _F5")]
        static void Open()
        {
            DisplayWizard<MapCheckWindow>("MapCheckWindow", "");
        }

        protected override bool DrawWizardGUI()
        {
            var res = base.DrawWizardGUI();
            if(GUILayout.Button("尝试寻找有效种子"))
            {
                TryFindValidSeed(mapkey);
            }
            if(GUILayout.Button("检测所有地图"))
            {
                CheckAllLevel();
            }
            
            return res;
        }

        private void CheckAllLevel()
        {
            var resPoolManager = EditorEnv.Service.GetService<ResPoolManager>();
            var data = resPoolManager.LoadResConfigDataNoCahe(ResFlag.Text_Level);
            var levelKeys = new List<string>(data.Keys);
            foreach (var key in levelKeys)
            {
                var levelData = data[key];
                bool isVaildLevel = !levelData.ContainsKey(JsonKeys.world) && !levelData.ContainsKey(JsonKeys.discard);
                if(isVaildLevel)
                {
                    TryFindValidSeed(key);
                }
            }
        }


        public void TryFindValidSeed(string key)
        {
            var content = EditorEnv.Service.GetService<ContentManager>();
            var resPoolManager = EditorEnv.Service.GetService<ResPoolManager>();
            var data = resPoolManager.LoadJsonDataNoCache(key, ResFlag.Text_Level);
            int width = 0, height = 0, mapSeed = 0, start = 0, end = 0; 
            string log = "";
            var isSuccess = true;
            Texture2D mapTex = null;
            try
            {
                // width = ((int)data[JsonKeys.width]);
                // height = ((int)data[JsonKeys.height]);
                mapSeed = (int)data[JsonKeys.mapSeed];
                //start = ((int)data[JsonKeys.start]);
                var mapTexKey = (string)data[JsonKeys.mapTex];
                mapTex = resPoolManager.LoadResWithKeyNoCache<Texture2D>(mapTexKey, ResFlag.Texture_Map);
            }
            catch (Exception e)
            {
                log += $"{key} format error!\n  erroer:{e}";
                isSuccess = false;
            }

            bool isNewSeed = false;
            if (isSuccess)
            {
                bool isReadSuccess = MapCreator.TryReadMapTexture(mapTex, out width, out height, out start, out var areaStartIdList, out end, out var grids);
                if (isReadSuccess)
                {
                    var originGrids = new MapGrid[grids.Length];
                    System.Array.Copy(grids, originGrids, grids.Length);
                    bool isCreateMap = MapCreator.TryCreateSimpleMap(grids, start, areaStartIdList, width, height, mapSeed);
                    if (!isCreateMap)
                    {
                        
                        float count = seedRange.y - seedRange.x;
                        for (int i = seedRange.x; i < seedRange.y; i++)
                        {
                            EditorUtility.DisplayProgressBar("地图检测中", $"地图检测中 {i} / {count}", i / count);
                            System.Array.Copy(originGrids, grids, grids.Length);
                            isCreateMap = MapCreator.TryCreateSimpleMap(grids, start, areaStartIdList, width, height, i);
                            if (isCreateMap)
                            {
                                log += $"{key}  seed {mapSeed} error! vaild seed:{i}";
                                isNewSeed = true;
                                break;
                            }
                        }
                        EditorUtility.ClearProgressBar();
                        if (!isCreateMap)
                            log += $"{key} seed error! can't create vaild seed";
                    }
                    else
                    {
                        log += $"{key}  pass";
                    }
                }
                else
                {
                    log += $"{key} mapTex error!";
                }

            }
            if(isNewSeed)
            {
                var asset = resPoolManager.LoadResWithKeyNoCache<TextAsset>(key, ResFlag.Text_Level);
                Debug.Log(log, asset);
            }
            else
            {
                Debug.Log(log);
            }

        }
    }

}
