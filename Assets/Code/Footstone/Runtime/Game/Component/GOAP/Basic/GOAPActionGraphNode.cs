using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Game
{
    public class GOAPActionGraphNode 
    {
        public int Id;
        public GOAPActionData Data;
        public List<int> DependNodeIndices = new();
        //两个节点之间的消耗
        public List<int> PathCosts = new();
        private const int bigCost = 99999;

        public GOAPActionGraphNode(GOAPActionData data, int id)
        {
            Data = data;
            Id = id;
        }

        public void CollectDepends(List<GOAPActionGraphNode> nodes)
        {
            DependNodeIndices.Clear();
            int nodeCount = nodes.Count;
            foreach (var node in nodes)
            {
                if(node.Id != this.Id && Data.IsDependOn(node.Data))
                {
                    DependNodeIndices.Add(node.Id);
                }
            }
        }

        public void CalculatePathCosts(List<GOAPActionGraphNode> nodes)
        {
            int[] costs = new int[nodes.Count];
            //bigCost意味着不应该走这条路
            for (int i = 0; i < nodes.Count; i++)
            {
                costs[i] = bigCost;
            }
            costs[this.Id] = 0;
            Stack<int> s = new Stack<int>();
            s.Push(this.Id);
            while (s.Count > 0)
            {
                var id = s.Pop();
                var pathCost = costs[id];
                foreach (var dependId in nodes[id].DependNodeIndices)
                {
                    var node = nodes[dependId];
                    var currentCost = pathCost + node.Data.Cost;
                    if(currentCost < costs[dependId])
                    {
                        costs[dependId] = currentCost;
                        s.Push(dependId);
                    }
                }
            }
            PathCosts.Clear();
            PathCosts.AddRange(costs);
        }

    }

}



