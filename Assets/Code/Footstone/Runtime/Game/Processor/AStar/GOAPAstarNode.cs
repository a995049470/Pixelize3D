using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Game
{
    public enum GOAPAstartNodePosition
    {
        None = 0,
        OpenList = 1,
        CloseList = 2,
        Fail = 3
    }

    public class GOAPAstarNode
    {
        public int Id { get; private set; }
        private GOAPActionGraphNode actionNode;
        private int actionCost { get => actionNode?.Data?.Cost ?? 0; }

        public GOAPAstarNode Parent { get; private set; }
        //真实消耗
        private float realCost = 0;
        //猜测的的剩余消耗
        private float vitrualRemainCost;
        //完成
        private float nodeVitrualRemainCost = 0;
        private GOAPStatusDictionary nodeRemainPreconditions = new();
        //从openlist里拿出来时才计算真正的前置条件
        private GOAPStatusDictionary realPreconditions = new();
        public GOAPStatusDictionary RealPreconditions { get => realPreconditions; }
        
        public GOAPAstartNodePosition NodePos;
        public float VitrualTotalCost { get => vitrualRemainCost + realCost; }
        private const int notCompleteCost = 10000;
        public bool IsVaild { get; private set; } = true;

        public GOAPAstarNode(GOAPGoalData goal)
        {
            Id = -1;
            realPreconditions.Add(goal.Target.Flag, goal.Target);
        }
        
        private float GetCost(Dictionary<GOAPStatusFlag, float> dic, GOAPStatusFlag flag)
        {
            return dic.GetValueOrDefault(flag, notCompleteCost);
        }

        

        public GOAPAstarNode(GOAPActionGraphNode action, GOAPStatusDictionary worldStatus, Dictionary<GOAPStatusFlag, float> costDic)
        {
            actionNode = action;
            Id = action.Id;
            if(action != null) realCost = action.Data.Cost;
            
            var preconditions = action.Data.Preconditions;
            nodeVitrualRemainCost = 0;
            NodePos = GOAPAstartNodePosition.None;
            if(preconditions != null)
            {
                foreach (var kvp in preconditions)
                {
                    if(!worldStatus.TryGetValue(kvp.Key, out var status) || !status.Pass(kvp.Value))
                    {
                        nodeRemainPreconditions.Add(kvp.Key, kvp.Value);
                        if(costDic.TryGetValue(kvp.Key, out var cost))
                        {
                            nodeVitrualRemainCost += GetCost(costDic, kvp.Key);
                        }
                        else
                        {
                            IsVaild = false;
                            break;
                        }
                    }
                }
            }
        }

        public static bool TryCraete(GOAPActionGraphNode action, GOAPStatusDictionary worldStatus, Dictionary<GOAPStatusFlag, float> costDic, out GOAPAstarNode node)
        {
            node = new(action, worldStatus, costDic);
            return node.IsVaild;
        }

        public void CalculateRealPreconditions(Dictionary<GOAPStatusFlag, float> costDic)
        {
            if(Parent != null)
            {
                var parentRealPreconditions = Parent.realPreconditions;
                nodeRemainPreconditions.CopyTo(realPreconditions);
                vitrualRemainCost = nodeVitrualRemainCost;
                foreach (var kvp in parentRealPreconditions)
                {
                    if(realPreconditions.TryGetValue(kvp.Key, out var status))
                    {
                        //TODO:思考条件之间的合并 
                        //UnityEngine.Debug.Log($"{kvp.Key} combine");
                        realPreconditions[kvp.Key] = status.Combine(kvp.Value);
                    }
                    else
                    {
                        realPreconditions[kvp.Key] = kvp.Value;
                        vitrualRemainCost += GetCost(costDic, kvp.Key);;
                    }
                }
                realPreconditions.Remove(actionNode.Data.EffectFlag);
                vitrualRemainCost -= GetCost(costDic, actionNode.Data.EffectFlag);
            }
        }

        public bool IsVaildParent(GOAPAstarNode node, Dictionary<GOAPStatusFlag, float> costDic)
        {
            bool isVaild = false;
            var currentParentRealCost = Parent?.realCost ?? int.MaxValue;
            var targetRealCost = node.realCost;
            if(Parent != node && targetRealCost < currentParentRealCost)  
            {
                isVaild = true;
                var parentRealPreconditions = node.realPreconditions;
                
                foreach (var kvp in parentRealPreconditions)
                {
                    if(nodeRemainPreconditions.TryGetValue(kvp.Key, out var status) && !kvp.Value.Pass(status))
                    {
                        isVaild = false;
                        break;
                    }
                }
            }
            return isVaild;
        }
        
        //TODO:需要判断是否有条件相互依赖的情况（不能有相互依赖）
        //暂时不考虑一个多个同类条件冲突的情况
        //同一个行为可能会清除多个条件
        // public bool TrySetParent(GOAPAstarNode parent, Dictionary<GOAPStatusFlag, float> costDic)
        // {
        //     bool isSuccess = false;
        //     var effect = actionNode.Data.Effect;
        //     if(parent.realPreconditions.TryGetValue(effect.Flag, out var status) && effect.Pass(status))
        //     {
        //         isSuccess = true;
        //         this.Parent = parent;
        //         this.realCost = CalculateCost(parent);
        //         vitrualRemainCost = nodeVitrualRemainCost + parent.vitrualRemainCost - GetCost(costDic, effect.Flag);
        //     }
        //     return isSuccess;
        // }



        public void SetVaildParent(GOAPAstarNode node, Dictionary<GOAPStatusFlag, float> costDic)
        {
            this.Parent = node;
            var effect = actionNode.Data.Effect;
            this.realCost = CalculateCost(node);
            vitrualRemainCost = nodeVitrualRemainCost + node.vitrualRemainCost - GetCost(costDic, effect.Flag);
        }

        
    
        

        private float CalculateCost(GOAPAstarNode node)
        {
            return actionCost + (node?.realCost ?? 0);
        }

    }
}
