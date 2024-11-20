using System.Collections.Generic;
using LitJson;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{   
    

    [DefaultEntityComponentProcessor(typeof(GOAPAgentProcessor))]
    public class GOAPAgentComponent : EntityComponent, IComponentSaveOrLoadCallback
    {
        
        [SerializeField]
        private GOAPGraphReference graphReference;
        private GOAPSerializableActionGraph serializedGraph { get => graphReference.Asset; }
        [SerializeField]
        private GOAPSortedGoalDataList serializedGoals;

        public Dictionary<GOAPStatusFlag, List<GOAPActionGraphNode>> ActionNodeDic { get => serializedGraph.ActionNodeDic; }
        public List<GOAPActionGraphNode> ActionNodes { get => serializedGraph.Nodes; }
        public List<GOAPGoalData> Goals { get => serializedGoals.Goals; }
        public Dictionary<GOAPStatusFlag, float> CostDic { get => serializedGraph.CostDic; } 
        public GOAPTask Task = new();
        [HideInInspector]
        public GOAPStatusDictionary WorldStatus = new();

        protected override void OnEnableRuntime()
        {
            base.OnEnableRuntime();
            //初始化部分状态 防止拿不到值

            
        }

        private void OnValidate() {
            //serializedGraph.SetDirty();
            //serializedGoals.SetDitry();
            // if(Application.isPlaying)
            // {
            //     //清理所有任务
            //     Task.Clear();
            //     //移除所有GOAPActionComponent
            //     Entity.RemoveComponents<GOAPActionComponent>();
            // }
        }

        public void OnBeforeSave()
        {
            WorldStatus.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            WorldStatus.OnAfterLoad();
        }
    }

}



