using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lost.Editor.Footstone
{
    public static class ShortcutKeyUtility
    {
        [MenuItem("GameObject/Lost/ChangeSelectionGameObjectActive %#w")]
        private static void ChangeSelectionGameObjectActive()
        {
            foreach (var go in Selection.gameObjects)
            {
                EditorUtility.SetDirty(go);
                Undo.RecordObject(go, "ChangeSelectionGameObjectActive");
                go.SetActive(!go.activeSelf);
            }
        }

        [MenuItem("GameObject/Lost/DebugLogForward")]
        private static void DebugLogForward()
        {
            if(Selection.activeTransform != null)
            {
                Debug.Log(Selection.activeTransform.forward);
            }
        }

        [MenuItem("GameObject/Lost/DebugLogBack")]
        private static void DebugLogBack()
        {
            if(Selection.activeTransform != null)
            {
                Debug.Log(-Selection.activeTransform.forward);
            }
        }
    }
}
