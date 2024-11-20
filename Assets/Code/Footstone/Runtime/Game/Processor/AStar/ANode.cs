using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public class ANode
    {
        public ANode parent;
        public Vector2Int id;
        private float g;
        public float h;
        public float f { get { return g + h; } }
        public ANode(Vector2Int end, Vector2Int pos, ANode node = null)
        {
            var offset = end - pos;
            id = pos;
            g = offset.magnitude;
            parent = node;
            h = GetH(parent);
        }

        public bool ChangeParent(ANode node)
        {
            float val = GetH(node);
            if (val >= h)
            {
                return false;
            }
            parent = node;
            h = val;
            return true;

        }
        public float GetH(ANode node)
        {
            if (node == null)
            {
                return 0;
            }
            float res = node.h + Vector2Int.Distance(node.id, id);
            return res;
        }
    }
}
