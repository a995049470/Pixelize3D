using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class AStar
    {   
        /// <summary>
        /// 找到起始点和目标点之间的路径
        /// 返回的路径列表 end -> start
        /// </summary>
        /// <returns></returns>
        public static bool TryFindWayToEnd(Vector2Int start, Vector2Int end, int[] map, int width, int height, int wayValue, out List<Vector2Int> pointList)
        {
            pointList = new List<Vector2Int>(16);
            var closeList = FindWay(start, end, map, width, height, wayValue);
            var temp = closeList[closeList.Count - 1];
            bool isSuccess = temp.id == end;
            if (isSuccess)
            {
                while (temp != null)
                {
                    pointList.Add(temp.id);
                    if (temp.id == start)
                    {
                        break;
                    }
                    temp = temp.parent;
                }
            }
            return isSuccess;
        }
        
        private static List<ANode> FindWay(Vector2Int start, Vector2Int end, int[] map, int width, int height, int wayValue)
        {
            Stack<Vector2Int> pointStack = new Stack<Vector2Int>();
            List<ANode> closelist = new List<ANode>();
            List<ANode> openlist = new List<ANode>();
            openlist.Add(new ANode(end, start));
            int len1 = width;
            int len2 = height;
            while (openlist.Count > 0)
            {
                var node = GetTopValue(openlist);
                closelist.Add(node);
                var id = node.id;
                if (id == end)
                {
                    break;
                }
                for (int i = id.x - 1; i < id.x + 2; i++)
                {
                    for (int j = id.y - 1; j < id.y + 2; j++)
                    {
                        if (i < 0 || i >= len1 || j < 0 || j >= len2)
                        {
                            continue;
                        }
                        if (i == id.x && j == id.y)
                        {
                            continue;
                        }
                        var n = map[i + j * width];
                        var isVaild = n == wayValue || (i == end.x && j == end.y);
                        if (!isVaild)
                        {
                            continue;
                        }
                        var v = new Vector2Int(i, j);
                        if (Vector2Int.Distance(v, id) > 1)
                        {
                            continue;
                        }
                        if (closelist.FindIndex(x => x.id == v) != -1)
                        {
                            continue;
                        }

                        AddNode(node, end, v, openlist);
                    }
                }
            }
            // var temp = closelist[closelist.Count - 1];
            // if (temp.id == end)
            // {
            //     while (temp != null)
            //     {
            //         if (temp.id == start)
            //         {
            //             break;
            //         }
            //         pointStack.Push(temp.id);
            //         //pointlist.Insert(0, temp.id);
            //         temp = temp.parent;
            //     }
            // }
            return closelist;
        }

        //改变或增加元素
        public static void AddNode(ANode parnt, Vector2Int end, Vector2Int pos, List<ANode> list)
        {
            int index = list.FindIndex(x => x.id == pos);
            ANode node = null;
            bool ischange = false;
            if (index == -1)
            {
                ischange = true;
                node = new ANode(end, pos, parnt);
                list.Add(node);
                index = list.Count - 1;
            }
            else
            {
                node = list[index];
                ischange = node.ChangeParent(parnt);
            }
            if (!ischange)
            {
                return;
            }
            while (index > 0)
            {
                var root = (index - 1) / 2;
                float v0 = list[root].f;
                float v1 = list[index].f;
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
        
        //从大小堆顶取值
        public static ANode GetTopValue(List<ANode> list)
        {
            ANode node = null;
            node = list[0];
            list[0] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            int root = 0;
            int left = 0;
            int right = 0;
            while (root < list.Count)
            {
                left = 2 * root + 1;
                right = 2 * root + 2;
                float v0 = list[root].f;
                float v1 = left >= list.Count ? float.MaxValue : list[left].f;
                float v2 = right >= list.Count ? float.MaxValue : list[right].f;
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

            return node;
        }

    }
}
