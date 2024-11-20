using System.Collections.Generic;
using Lost.Runtime.Footstone.Collection;

namespace Lost.Runtime.Footstone.Core
{
    public class UpdateableCollection : OrderedCollection<IUpdateable>
    {
        public UpdateableCollection() : this(4)
        {
        }

        public UpdateableCollection(int capacity) : base(IUpdateableComparer.Default, capacity)
        {
        }

        private class IUpdateableComparer : Comparer<IUpdateable>
        {
            public static new readonly IUpdateableComparer Default = new IUpdateableComparer();

            public override int Compare(IUpdateable x, IUpdateable y)
            {
                return x.UpdateOrder.CompareTo(y.UpdateOrder);
            }
        }   
    }
}

