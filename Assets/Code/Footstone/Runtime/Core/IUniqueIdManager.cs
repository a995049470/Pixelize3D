namespace Lost.Runtime.Footstone.Core
{
    public interface IUniqueIdManager
    {
        public ulong InvalidId { get; }
        public ulong CreateUniqueId();
        public void RecycleUniqueId(ref ulong id);
        public bool IsVaild(ulong id);
    }
}



