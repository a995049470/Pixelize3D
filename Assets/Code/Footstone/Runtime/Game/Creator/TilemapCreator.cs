using System;
using System.Collections.Generic;
using LitJson;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class TileModel
    {
        public string Key;
        public int SubTerrienHash;
        public int Rotation = 0;
        public int FlipX = 0;
        public int FlipY = 0;
        
     
        private static TileModel Rotate90(TileModel src)
        {
            var hash_src = src.SubTerrienHash;
            var a = (hash_src >> 0) & 255;
            var b = (hash_src >> 8) & 255;
            var c = (hash_src >> 16) & 255;
            var d = (hash_src >> 24) & 255;
            var hash_dst = (d << 0) | (a << 8) | (b << 16) | (c << 24);
            var dst = new TileModel(src.Key, hash_dst);
            dst.Rotation = src.Rotation + 90;
            dst.FlipX = src.FlipX;
            dst.FlipY = src.FlipY;
            return dst;
        }

        private static TileModel ModelFlipX(TileModel src)
        {
            var hash_src = src.SubTerrienHash;
            var a = (hash_src >> 0) & 255;
            var b = (hash_src >> 8) & 255;
            var c = (hash_src >> 16) & 255;
            var d = (hash_src >> 24) & 255;
            var hash_dst = (b << 0) | (a << 8) | (d << 16) | (c << 24);
            var dst = new TileModel(src.Key, hash_dst);
            dst.Rotation = src.Rotation;
            dst.FlipX = 1 - src.FlipX;
            dst.FlipY = src.FlipY;
            return dst;
        }

        private static TileModel ModelFlipY(TileModel src)
        {
            var hash_src = src.SubTerrienHash;
            var a = (hash_src >> 0) & 255;
            var b = (hash_src >> 8) & 255;
            var c = (hash_src >> 16) & 255;
            var d = (hash_src >> 24) & 255;
            var hash_dst = (d << 0) | (c << 8) | (b << 16) | (a << 24);
            var dst = new TileModel(src.Key, hash_dst);
            dst.Rotation = src.Rotation;
            dst.FlipX = src.FlipX;
            dst.FlipY = 1 - src.FlipY;
            return dst;
        }

        public static List<TileModel> CreateTileModels(string key, int hash, bool isRotate, bool isFlip)
        {
            var models = new List<TileModel>(12);
            var model = new TileModel(key, hash);
            for (int i = 0; i < 12; i++)
            {
                if(isRotate && isFlip)
                {
                    models.Add(model);
                }
                else if(isRotate)
                {
                    if(i < 4) models.Add(model);
                }
                else if(isFlip)
                {
                    if(i % 4 == 0) models.Add(model);
                }
                else
                {
                    if(i == 0) models.Add(model);
                }
                model = Rotate90(model);
                if(i == 3)
                {
                    model = ModelFlipX(model);
                }
                else if(i == 7)
                {
                    model = ModelFlipX(model);
                    model = ModelFlipY(model);
                }
            }
            return models;
            
        }

        public TileModel()
        {
        }

        public TileModel(string key, int hash)
        {
            Key = key;
            SubTerrienHash = hash;
        }   

        public Vector4 GetRotateFlipParameter()
        {
            return new Vector4(
                FlipX, FlipY, Rotation, 0
            );
        }

        public TileModel Clone()
        {
            return new TileModel()
            {
                Key = this.Key,
                SubTerrienHash = this.SubTerrienHash,
                FlipX = this.FlipX,
                FlipY = this.FlipY,
                Rotation = this.Rotation
            };
        }

        
    }
    //地块配置绕序
    //  0 1 
    //  3 2
    public class TilemapCreator 
    {
        private MapGrid[] grids;
        private int width;
        private int height;
        private Dictionary<int, List<TileModel>> tilesDic;
        private Random random;

        private static int ConvertSubTerrainToInt(int[] subTerrian)
        {
            var hash = (subTerrian[0] << 0) | 
                       (subTerrian[1] << 8) | 
                       (subTerrian[2] << 16)|
                       (subTerrian[3] << 24);
            return hash;
        }

        public static Dictionary<int, List<TileModel>> CreateTilesDic(JsonData tileData)
        {
            var dic = new Dictionary<int, List<TileModel>>();
            foreach (var key in tileData.Keys)
            {
                var json = tileData[key][JsonKeys.sub_terrain].ToJson();
                var subTerrian = JsonMapper.ToObject<int[]>(json);
                var hash = ConvertSubTerrainToInt(subTerrian);
                var tileModels = TileModel.CreateTileModels(key, hash, false, true);
                foreach (var model in tileModels)
                {
                    if(!dic.TryGetValue(model.SubTerrienHash, out var list))
                    {
                        list = new();
                        dic[model.SubTerrienHash] = list;
                    }
                    list.Add(model);
                }
            }
            return dic;
        }

        public TilemapCreator()
        {
            
        }

        public TilemapCreator(MapGrid[] _grids, int _width, int _height, JsonData _tileData, Random _random)
        {
            var dic = CreateTilesDic(_tileData);
            Initialize(_grids, _width, _height, dic, _random);
        }

        public TilemapCreator(MapGrid[] _grids, int _width, int _height, Dictionary<int, List<TileModel>> dic, Random _random)
        {
            Initialize(_grids, _width, _height, dic, _random);
        }

        public void Initialize(MapGrid[] _grids, int _width, int _height, Dictionary<int, List<TileModel>> dic, Random _random)
        {
            grids = _grids;
            width = _width;
            height = _height;
            tilesDic = dic;
            random = _random;
        }
        


        private bool IsVaild(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private int SapmleTerrainHash(int x, int y)
        {
            var subTerrian = new int[4];
            var t = x + y * width;
            if(IsVaild(x - 1, y)) subTerrian[0] = grids[t - 1].TerrainId;
            if(IsVaild(x, y)) subTerrian[1] = grids[t].TerrainId;
            if(IsVaild(x, y - 1)) subTerrian[2] = grids[t - width].TerrainId;
            if(IsVaild(x - 1, y - 1)) subTerrian[3] = grids[t - width - 1].TerrainId;

            var hash = ConvertSubTerrainToInt(subTerrian);
            return hash;
        }

        

        public bool TryCreateTiles(out TileModel[] textureKeys, out int tilemapWidth, out int tilemapHeight, out int[] terrainHashs)
        {
            bool isSuccess = true;
            tilemapWidth = width + 1;
            tilemapHeight = height + 1;
            var tileCount = tilemapWidth * tilemapHeight;
            textureKeys = new TileModel[tileCount];
            terrainHashs = new int[tileCount]; 
            for (int i = 0; i < tileCount; i++)
            {
                terrainHashs[i] = SapmleTerrainHash(i % tilemapWidth, i / tilemapWidth);
            }
            for (int i = 0; i < tileCount; i++)
            {
                var hash = terrainHashs[i];
                if(!tilesDic.TryGetValue(hash, out var list) || list.Count == 0)
                {
                    isSuccess = false;
                    textureKeys[i] = null;
                    //break;
                    UnityEngine.Debug.LogError("Not Found Texture");
                }
                else
                {
                    var index = random.Next(0, list.Count);
                    textureKeys[i] = list[index];
                }
            }
            return isSuccess;
        }
    }




}
