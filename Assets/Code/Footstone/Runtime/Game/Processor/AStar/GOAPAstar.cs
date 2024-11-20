using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Game
{

    public class GOAPAstar
    {
        private List<GOAPActionGraphNode> actionList;
        private Dictionary<GOAPStatusFlag, List<GOAPActionGraphNode>> actionNodeDic;
        private GOAPStatusDictionary worldStatus;
        private Dictionary<GOAPStatusFlag, float> costDic;
        private GOAPGoalData goal;

        

        public bool TryFindNodeIndiceToComplteGoal(GOAPAgentComponent agentComponent)
        {
            actionList = agentComponent.ActionNodes;
            actionNodeDic = agentComponent.ActionNodeDic;
            worldStatus = agentComponent.WorldStatus;
            costDic = agentComponent.CostDic;
            bool isFindGoal = false;
            foreach (var _goal in agentComponent.Goals)
            {
                if(_goal.IsAchievable(worldStatus))
                {
                    isFindGoal = true;
                    goal = _goal;
                    break;
                }
            }
            List<int> indices;
            if(isFindGoal)
            {
                indices = FindNodeIndiceToCompleteGoal();
            }
            else
            {
                indices = new();
            }
            bool isSuccess  = indices.Count > 0;

            if(isSuccess) agentComponent.Task.StartTask(indices);

            actionList = null;
            actionNodeDic = null;
            worldStatus = null;
            costDic = null;
            goal = null;
            return isSuccess;
        }

        private List<int> FindNodeIndiceToCompleteGoal()
        {
            var astarNodeDic = new Dictionary<int, GOAPAstarNode>();
            var root = new GOAPAstarNode(goal);
            var openList = new List<GOAPAstarNode>();
            openList.Add(root);
            //var closeList = new List<GOAPAstarNode>();
            var isTaskSuccess = false;
            GOAPAstarNode start = null;
            while (openList.Count > 0 && !isTaskSuccess)
            {
                var astarNode = GetTopValue(openList);
                astarNode.CalculateRealPreconditions(costDic);
                if(astarNode.RealPreconditions.Count == 0)
                {
                    isTaskSuccess = true;
                    start = astarNode;
                    break;
                }

                var cacheNodes = new List<GOAPAstarNode>(16);
                bool isNodeComplte = true;
                foreach (var kvp in astarNode.RealPreconditions)
                {
                    bool isFindAction = false;
                    if(actionNodeDic.TryGetValue(kvp.Key, out var nodeList))
                    {
                        foreach (var actionNode in nodeList)
                        {
                            if(!astarNodeDic.TryGetValue(actionNode.Id, out var node))
                            {
                                node = new GOAPAstarNode(actionNode, worldStatus, costDic);
                                astarNodeDic[astarNode.Id] = node;
                            }
                            if(node.IsVaild)
                            {
                                var originTotalCost = node.VitrualTotalCost;
                                bool isActiveNode = node.NodePos == GOAPAstartNodePosition.OpenList || node.NodePos == GOAPAstartNodePosition.None;
                                
                                if(isActiveNode && node.IsVaildParent(astarNode, costDic))
                                {
                                    isFindAction = true;
                                    cacheNodes.Add(node); 
                                }
                            }
                        }
                    }

                    if(!isFindAction)
                    {
                        isNodeComplte = false;
                        break;
                    }   
                }
                if(isNodeComplte)
                {
                    astarNode.NodePos = GOAPAstartNodePosition.CloseList;
                    foreach (var node in cacheNodes)
                    {
                        
                        if(node.NodePos == GOAPAstartNodePosition.None)
                        {
                            var index = openList.Count;
                            openList.Add(node);
                            node.SetVaildParent(astarNode, costDic);
                            ReSort(index, openList, false);
                        }
                        else
                        {
                            var originTotalCost = node.VitrualTotalCost;
                            node.SetVaildParent(astarNode, costDic);
                            var currentTotalCost = node.VitrualTotalCost;
                           
                            if(originTotalCost != currentTotalCost)
                            {   
                                var index = openList.IndexOf(node);
                                ReSort(index, openList, currentTotalCost > originTotalCost);
                            }
                        }
                    }
                }
                else
                {
                    astarNode.NodePos = GOAPAstartNodePosition.Fail;
                }
                
            }
            
            var indices = new List<int>(8);
            if(isTaskSuccess)
            {
                var temp = start;
                while (temp != null)
                {
                    if(temp.Id >= 0)
                        indices.Add(temp.Id);
                    temp = temp.Parent;
                }
            }
            return indices;
        }

        //改变或增加元素
        public void ReSort(int index, List<GOAPAstarNode> list, bool isBigger)
        {
            if(isBigger)
                SortBigger(index, list);
            else
                SortSmaller(index, list);
        }
        
        /// <summary>
        /// 数值变小时重新排序
        /// </summary>
        /// <param name="index"></param>
        /// <param name="list"></param>
        public void SortSmaller(int index, List<GOAPAstarNode> list)
        {
            while (index > 0)
            {
                var root = (index - 1) / 2;
                float v0 = list[root].VitrualTotalCost;
                float v1 = list[index].VitrualTotalCost;
                if (v0 < v1)
                {
                    break;
                }
                var temp = list[root];
                list[root] = list[index];
                list[index] = temp;
                index = root;
            }
        }

        /// <summary>
        /// 数值增大时重新排序
        /// </summary>
        /// <param name="index"></param>
        /// <param name="list"></param>
        public void SortBigger(int index, List<GOAPAstarNode> list)
        {
            int root = index;
            int left = 0;
            int right = 0;
            while (root < list.Count)
            {
                left = 2 * root + 1;
                right = 2 * root + 2;
                float v0 = list[root].VitrualTotalCost;
                float v1 = left >= list.Count ? float.MaxValue : list[left].VitrualTotalCost;
                float v2 = right >= list.Count ? float.MaxValue : list[right].VitrualTotalCost;
                if (v1 <= v0 && v1 <= v2)
                {
                    var temp = list[root];
                    list[root] = list[left];
                    list[left] = temp;
                    root = left;
                }
                else if (v2 <= v0 && v2 <= v1)
                {
                    var temp = list[root];
                    list[root] = list[right];
                    list[right] = temp;
                    root = right;
                }
                else
                {
                    break;
                }
            }

        }

        //从大小堆顶取值
        public GOAPAstarNode GetTopValue(List<GOAPAstarNode> list)
        {
            GOAPAstarNode node = null;
            node = list[0];
            list[0] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            int root = 0;
            SortBigger(root, list);
            return node;
        }
    }
}
