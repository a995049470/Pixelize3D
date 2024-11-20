using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Lost/GOAP/Graph")]
    public class GOAPSerializableActionGraph : ScriptableObject
    {
        [SerializeField]
        private List<GOAPActionData> actionDatas = new();
        private bool isDitryNodes = true;
        private bool isDitryNodeDic = true;
        private bool isDitryCostDic = true;
        private Dictionary<GOAPStatusFlag, List<GOAPActionGraphNode>> actionNodeDic = new();
        private List<GOAPActionGraphNode> nodes = new();
        private Dictionary<GOAPStatusFlag, float> costDic = new();
        public List<GOAPActionGraphNode> Nodes
        {
            get
            {
                if(isDitryNodes)
                {
                    isDitryNodes = false;
                    nodes.Clear();
                    foreach (var data in actionDatas)
                    {
                        var id = nodes.Count;
                        nodes.Add(new(data, id));
                    }
                    foreach (var node in nodes)
                    {
                        node.CollectDepends(nodes);
                    }
                }
                return nodes;
            }
        }
        public Dictionary<GOAPStatusFlag, List<GOAPActionGraphNode>> ActionNodeDic 
        {
            get
            {
                if(isDitryNodeDic)
                {
                    isDitryNodeDic = false;
                    actionNodeDic.Clear();
                    foreach (var node in Nodes)
                    {
                        var flag = node.Data.EffectFlag;
                        if(!actionNodeDic.TryGetValue(flag, out var list))
                        {
                            list = new();
                            actionNodeDic[flag] = list;
                        }
                        list.Add(node);
                    }
                }
                return actionNodeDic;
            }
        }
        public Dictionary<GOAPStatusFlag, float> CostDic
        {
            get
            {
                if(isDitryCostDic)
                {
                    isDitryCostDic = false;
                    costDic.Clear();
                    foreach (var kvp in ActionNodeDic)
                    {
                        var sum = 0.0f;
                        kvp.Value.ForEach(node => sum += node.Data.Cost);
                        var v = sum / Mathf.Max(1, kvp.Value.Count);
                        costDic[kvp.Key] = v;
                    }
                }
                return costDic;
            }
        }



        public GOAPSerializableActionGraph()
        {
            isDitryNodes = true;
            isDitryNodeDic = true;
            isDitryCostDic = true;
        }

        private void OnValidate() {
            isDitryNodes = true;
            isDitryNodeDic = true;
            isDitryCostDic = true;
        }
    
        // public void SetDirty()
        // {
        //     isDitry = true;
        // }
    }

}



