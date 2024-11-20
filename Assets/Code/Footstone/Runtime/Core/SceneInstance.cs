using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Lost.Runtime.Footstone.Collection;

namespace Lost.Runtime.Footstone.Core
{

    public sealed class SceneInstance : EntityManager
    {
        private Scene rootScene;
        /// <summary>
        /// Occurs when the scene changed from a scene child component.
        /// </summary>
        public event EventHandler<EventArgs> RootSceneChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityManager" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public SceneInstance(IServiceRegistry service) : this(service, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneInstance" /> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="rootScene">The scene entity root.</param>
        /// <param name="executionMode">The mode that determines which processors are executed.</param>
        /// <exception cref="System.ArgumentNullException">services
        /// or
        /// rootScene</exception>
        public SceneInstance(IServiceRegistry service, Scene rootScene) : base(service)
        {
            RootScene = rootScene;
        }

        /// <summary>
        /// Gets the scene.
        /// </summary>
        /// <value>The scene.</value>
        public Scene RootScene
        {
            get { return rootScene; }
            set
            {
                if (rootScene == value)
                    return;

                if (rootScene != null)
                {
                    Remove(rootScene);
                }

                // set rootScene before adding entities because component processors might want to reference it
                rootScene = value;

                if (value != null)
                {
                    Add(value);
                }

                OnRootSceneChanged();
            }
        }

        public void Destroy()
        {
            RootScene = null;

            // Cleaning processors should not be necessary anymore, but physics are not properly cleaned up otherwise
            //Reset();

        }

        // /// <summary>
        // /// Gets the current scene valid only from a rendering context. May be null.
        // /// </summary>
        // /// <param name="context">The context.</param>
        // /// <returns>Stride.Engine.SceneInstance.</returns>
        // public static SceneInstance GetCurrent(RenderContext context)
        // {
        //     return context.Tags.Get(Current);
        // }

        private void Add(Scene scene)
        {
            if (scene.Entities.Count > 0)
            {
                var entitiesToAdd = new FastList<Entity>();
                // Reverse order, we're adding and removing from the tail to 
                // avoid forcing the list to move all items when removing at [0]
                for (int i = scene.Entities.Count -1; i >= 0; i-- )
                    entitiesToAdd.Add(scene.Entities[i]);

                scene.Entities.CollectionChanged += DealWithTempChanges;
                while (entitiesToAdd.Count > 0)
                {
                    int i = entitiesToAdd.Count - 1;
                    var entity = entitiesToAdd[i];
                    entitiesToAdd.RemoveAt(i);
                    Add(entity);
                }
                scene.Entities.CollectionChanged -= DealWithTempChanges;

                void DealWithTempChanges(object sender, TrackingCollectionChangedEventArgs e)
                {
                    Entity entity = (Entity)e.Item;
                    if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        if (entitiesToAdd.Remove(entity) == false)
                            Remove(entity);
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Add)
                        entitiesToAdd.Add(entity);
                }
            }

            // if (scene.Children.Count > 0)
            // {
            //     var scenesToAdd = new FastList<Scene>();
            //     // Reverse order, we're adding and removing from the tail to 
            //     // avoid forcing the list to move all items when removing at [0]
            //     for (int i = scene.Children.Count - 1; i >= 0; i--)
            //         scenesToAdd.Add(scene.Children[i]);

            //     scene.Children.CollectionChanged += DealWithTempChanges;
            //     while (scenesToAdd.Count > 0)
            //     {
            //         int i = scenesToAdd.Count - 1;
            //         var entity = scenesToAdd[i];
            //         scenesToAdd.RemoveAt(i);
            //         Add(entity);
            //     }
            //     scene.Children.CollectionChanged -= DealWithTempChanges;

            //     void DealWithTempChanges(object sender, TrackingCollectionChangedEventArgs e)
            //     {
            //         Scene subScene = (Scene)e.Item;
            //         if (e.Action == NotifyCollectionChangedAction.Remove)
            //         {
            //             if (scenesToAdd.Remove(subScene) == false)
            //                 Remove(subScene);
            //         }
            //         else if (e.Action == NotifyCollectionChangedAction.Add)
            //             scenesToAdd.Add(subScene);
            //     }
            // }

            // // Listen to future changes in entities and child scenes
            // scene.Children.CollectionChanged += Children_CollectionChanged;
            scene.Entities.CollectionChanged += Entities_CollectionChanged;
        }

        private void Remove(Scene scene)
        {
            scene.Entities.CollectionChanged -= Entities_CollectionChanged;
            // scene.Children.CollectionChanged -= Children_CollectionChanged;

            // if (scene.Children.Count > 0)
            // {
            //     var scenesToRemove = new Scene[scene.Children.Count];
            //     scene.Children.CopyTo(scenesToRemove, 0);
            //     foreach (var childScene in scenesToRemove)
            //         Remove(childScene);
            // }

            if (scene.Entities.Count > 0)
            {
                var entitiesToRemove = new Entity[scene.Entities.Count];
                scene.Entities.CopyTo(entitiesToRemove, 0);
                foreach (var entity in entitiesToRemove)
                    Remove(entity);
            }
        }

        private void Entities_CollectionChanged(object sender, TrackingCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Add((Entity)e.Item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove((Entity)e.Item);
                    break;
            }
        }

        private void Children_CollectionChanged(object sender, TrackingCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Add((Scene)e.Item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove((Scene)e.Item);
                    break;
            }
        }

        
        

       


        private void OnRootSceneChanged()
        {
            RootSceneChanged?.Invoke(this, EventArgs.Empty);
        }

    
    }
}



