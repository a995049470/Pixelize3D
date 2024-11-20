using UnityEngine;
using UnityEditor;
using Lost.Render.Runtime;

namespace Lost.Editor.Footstone
{
    [CustomEditor(typeof(ObjectPositionController))]
    public class ObjectPositionControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("FindRenderers"))
            {
                (target as ObjectPositionController).FindRenderers();
            }
        }
    }
}
