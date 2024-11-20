using System;


namespace Lost.Runtime.Footstone.Core
{
    public class TrackingEntityProcessorCollection : EntityProcessorCollection
    {
        private readonly EntityManager manager;

        public TrackingEntityProcessorCollection(EntityManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            this.manager = manager;
        }

        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
            {
                manager.OnProcessorRemoved(this[i]);
            }

            base.ClearItems();
        }

        protected override void AddItem(EntityProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));
            if (!Contains(processor))
            {
                base.AddItem(processor);
                manager.OnProcessorAdded(processor);
            }
        }

        protected override void RemoteItem(int index)
        {
            var processor = this[index];
            base.RemoteItem(index);
            manager.OnProcessorRemoved(processor);
        }
    }
}

