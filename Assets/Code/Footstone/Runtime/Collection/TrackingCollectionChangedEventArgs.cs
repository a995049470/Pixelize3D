// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using System.Collections.Specialized;

namespace Lost.Runtime.Footstone.Collection
{
    public class TrackingCollectionChangedEventArgs : EventArgs
    {
        public TrackingCollectionChangedEventArgs(NotifyCollectionChangedAction action, object item, object oldItem, int index, bool collectionChanged)
        {
            Action = action;
            Item = item;
            OldItem = oldItem;
            Index = index;
            CollectionChanged = collectionChanged;
        }

        public TrackingCollectionChangedEventArgs(NotifyCollectionChangedAction action, object key, object item, object oldItem, bool collectionChanged)
        {
            Action = action;
            Item = item;
            OldItem = oldItem;
            Key = key;
            Index = -1;
            CollectionChanged = collectionChanged;
        }
        
        /// <summary>
        /// Gets the type of action performed.
        /// Allowed values are <see cref="NotifyCollectionChangedAction.Add"/> and <see cref="NotifyCollectionChangedAction.Remove"/>.
        /// </summary>
        public NotifyCollectionChangedAction Action { get; private set; }
        
        /// <summary>
        /// Gets the added or removed item (if dictionary, value only).
        /// </summary>
        public object Item { get; private set; }

        /// <summary>
        /// Gets the previous value. Only valid if <see cref="Action"/> is <see cref="NotifyCollectionChangedAction.Add"/> and <see cref=""/>
        /// </summary>
        public object OldItem { get; private set; }

        /// <summary>Gets the added or removed key (if dictionary).</summary>
        public object Key { get; private set; }
        
        /// <summary>
        /// Gets the index in the collection (if applicable).
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [collection changed (not a replacement but real insertion/removal)].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [collection changed]; otherwise, <c>false</c>.
        /// </value>
        public bool CollectionChanged { get; private set; }
    }
}
