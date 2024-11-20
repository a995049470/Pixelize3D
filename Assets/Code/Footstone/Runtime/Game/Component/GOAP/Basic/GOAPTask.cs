using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class GOAPTask
    {       
        public enum GOAPActionStatus
        {
            None = 0,
            Executing = 1,
            Success = 2,
            Fail = 3,
        }

        [SerializeField]
        private List<int> actionNodeIdList = new();
        [SerializeField]
        private int ptr = 0;
        [SerializeField]
        private GOAPActionStatus actionStatus = GOAPActionStatus.None;
        [SerializeField]
        private int lastCompletePtr = -1;
        
        public bool TryCompleteTask(CommandBuffer cmd, Entity entity, List<GOAPActionGraphNode> nodes, GOAPStatusDictionary worldStatus)
        {
            int currentCompletePtr = -1;
            if(actionStatus == GOAPActionStatus.Success || actionStatus == GOAPActionStatus.Fail)
            {
                currentCompletePtr = ptr;
            }
            bool isWaitExecuteCurrentAction = actionStatus == GOAPActionStatus.None && (ptr < actionNodeIdList.Count);
            bool isExecuteNextAction = actionStatus == GOAPActionStatus.Success && (ptr < actionNodeIdList.Count - 1);
            bool isExecutingAction = actionStatus == GOAPActionStatus.Executing;
            if(isWaitExecuteCurrentAction)
            {
                actionStatus = GOAPActionStatus.Executing;
                nodes[actionNodeIdList[ptr]].Data.EnableGOAPActionComponent(cmd, entity);
            }
            else if(isExecuteNextAction)
            {
                ptr += 1;
                actionStatus = GOAPActionStatus.Executing;
                nodes[actionNodeIdList[ptr]].Data.EnableGOAPActionComponent(cmd, entity);
            }
            else if(isExecutingAction)
            {
                var data = nodes[actionNodeIdList[ptr]].Data;
                if(data.IsFail(worldStatus))
                {
                    currentCompletePtr = ptr;
                    //data.DisableGOAPActionComponent(cmd, entity);
                    actionStatus = GOAPActionStatus.Fail;
                }
            }
            if(currentCompletePtr >= 0 && currentCompletePtr != lastCompletePtr)
            {
                lastCompletePtr = currentCompletePtr;
                var data = nodes[actionNodeIdList[currentCompletePtr]].Data;
                data.DisableGOAPActionComponent(cmd, entity);
            }
            bool isTaskCompelte = actionStatus != GOAPActionStatus.Executing;
            return isTaskCompelte;
        } 

        public void FinshCurrentAction(bool isSuccess)
        {
            actionStatus = isSuccess ? GOAPActionStatus.Success : GOAPActionStatus.Fail;
        }

        public void StartTask(IEnumerable<int> indices)
        {
            lastCompletePtr = -1;
            ptr = 0;
            actionNodeIdList.Clear();
            actionNodeIdList.AddRange(indices);
            actionStatus = GOAPActionStatus.None;
        }

        public void Clear()
        {
            ptr = 0;
            lastCompletePtr = -1;
            actionStatus = GOAPActionStatus.None;
        }
    }   

}



