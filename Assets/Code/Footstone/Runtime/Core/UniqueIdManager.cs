using System.Collections.Generic;

namespace Lost.Runtime.Footstone.Core
{

    public class UniqueIdManager 
    {
        private uint magic = 1;
        private uint max = 0;
        public uint Magic { get => magic; }
        public uint Max { get => max; }
        
        private Stack<uint> stack = new Stack<uint>(256);
        //无效Id
        public readonly ulong InvalidId = 0;
        
        public uint[] GetRemainNums()
        {
            return stack.ToArray();
        }

        public void Set(uint _magic, uint _max, uint[] _nums)
        {
            magic = _magic;
            max = _max;
            stack.Clear();
            stack = new Stack<uint>(_nums);
        }

        public ulong CreateUniqueId()
        {
            if(!stack.TryPop(out var id))
            {
                id = max;
                max = max == uint.MaxValue ? 0 : max + 1;
            }
            
            var uid = id | ((ulong)magic << 32);
            magic = magic == uint.MaxValue ? 1 : magic + 1;
            return uid;
        }

        public void RecycleUniqueId(ref ulong id)
        {
            if(id != InvalidId)
            {
                stack.Push((uint)(id & uint.MaxValue));
                id = InvalidId;
            }
        }

        /// <summary>
        /// 检查Id是否和无效值不相等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsVaild(ulong id)
        {
            return id != InvalidId;
        }


    }
}



