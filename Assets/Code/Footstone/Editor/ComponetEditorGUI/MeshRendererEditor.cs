using UnityEngine;
using UnityEditor;

namespace Lost.Editor.Footstone
{
    [CustomEditor(typeof(MeshRenderer))]
    public class MeshRendererEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var renderer = target as MeshRenderer;
            if(renderer)
            {
                var sortingOrder = EditorGUILayout.IntField("sortingOrder:", renderer.sortingOrder);
                if (sortingOrder != renderer.sortingOrder)
                {
                    renderer.sortingOrder = sortingOrder;
                    EditorUtility.SetDirty(renderer);
                }
            }
        }
    }
}
