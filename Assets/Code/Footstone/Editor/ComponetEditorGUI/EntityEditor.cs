using UnityEngine;
using UnityEditor;
using Lost.Runtime.Footstone.Core;

namespace Lost.Editor.Footstone
{
    [CustomEditor(typeof(Entity))]
    public class EntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("ClearUID"))
            {
                ClearAllUID(target as Entity);
            }
        }

        public void ClearAllUID(Entity entity)
        {
            var entityComponents = entity.GetComponentsInChildren<EntityComponent>();
            if(entity.Id != 0)
            {
                entity.Id = 0;
                EditorUtility.SetDirty(entity);
            }
            foreach (var comp in entityComponents)
            {
                if(comp.Id != 0)
                {
                    comp.Id = 0;
                    EditorUtility.SetDirty(comp);
                }
                
            }
            
        }
    }
}
