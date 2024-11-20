using System.Collections.Generic;
using LitJson;

namespace Lost.Runtime.Footstone.Game
{
    //祭品数据
    [System.Serializable]
    public class OblationData
    {
        public string Key;
        public int RealCount;
        public int TotalExp;
        private JsonData cacheData;
        public JsonData ItemData { get => cacheData; }
        private bool isDitry = true;
        private int cacheLevel = 0;

        public OblationData()
        {

        }

        public OblationData(string key, ResPoolManager resPoolManager)
        {
            Key = key;
            TotalExp = 0;
            RealCount = 0;
            cacheData = resPoolManager.LoadItemData(Key);
        }

        private int GetCurrentLevel()
        {
            if(isDitry)
            {
                isDitry = false;
                var rewards = cacheData[JsonKeys.rewards_stage];
                for (int i = 0; i < rewards.Count; i++)
                {
                    var data = rewards[i];
                    var exp = (int)data[JsonKeys.exp];
                    if(TotalExp >= exp)
                        cacheLevel = i + 1;
                    else
                        break;
                }
            }
            return cacheLevel;
        }

        public bool TryGetStageExp(out int exp)
        {
            var level = GetCurrentLevel();
            var stageRewards = cacheData[JsonKeys.rewards_stage];
            bool isGet = level < stageRewards.Count;
            if(isGet)
            {
                var data = stageRewards[level];
                exp = (int)data[JsonKeys.exp];
            }
            else
            {
                exp = -1;
            }
            return isGet;
        }

        public bool TryGetStageRewards(out (string, int) rewards)
        {
            var level = GetCurrentLevel();
            var stageRewards = cacheData[JsonKeys.rewards_stage];
            bool isGet = level < stageRewards.Count;
            if(isGet)
            {
                var data = stageRewards[level];
                rewards = ((string)data[JsonKeys.name], (int)data[JsonKeys.count]);
            }
            else
            {
                rewards = new();
            }
            return isGet;
        }

        public (string, int) GetBaseRewards()
        {
            var baseRewards = cacheData[JsonKeys.rewards_base];
            return (
                (string)baseRewards[JsonKeys.name],
                (int)baseRewards[JsonKeys.count]
            );
        }

        //获得献祭奖励
        public (string, int)[] GetRewards(int count, int exp)
        {
            var originLevel = GetCurrentLevel();
            RealCount += count;
            TotalExp += exp;
            isDitry = true;
            var currentLevel = GetCurrentLevel();
            var rewardCount = currentLevel - originLevel + 1;
            var result = new (string, int)[rewardCount];
            if(currentLevel > originLevel)
            {
                var stageRewards = cacheData[JsonKeys.rewards_stage];
                for (int i = originLevel; i < currentLevel; i++)
                {
                    var data = stageRewards[i];
                    var index = i - originLevel;
                    result[index] = ((string)data[JsonKeys.name], (int)data[JsonKeys.count]);
                }
            }
            {
                var baseRewards = cacheData[JsonKeys.rewards_base];
                result[rewardCount - 1] = (
                    (string)baseRewards[JsonKeys.name],
                    (int)baseRewards[JsonKeys.count] * count
                );
            }
            return result;
        }

        public void OnAfterLoad(ResPoolManager resPoolManager)
        {
            cacheData = resPoolManager.LoadItemData(Key);
        }
    }

}



