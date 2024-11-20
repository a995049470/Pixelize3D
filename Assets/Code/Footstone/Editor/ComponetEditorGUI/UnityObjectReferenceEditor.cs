using UnityEngine;
using UnityEditor;
using Lost.Runtime.Footstone.Game;
using UnityEngine.UIElements;
using Lost.Runtime.Footstone.Core;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Reflection;

namespace Lost.Editor.Footstone
{
    [CustomPropertyDrawer(typeof(UnityObjectReference), true)]
    public class UnityObjectReferenceEditor : PropertyDrawer
    {
        public static bool IsArrayElement(SerializedProperty property)
        {
            // 检查 propertyPath 是否包含 ".Array.data["
            bool isArrayElement = property.propertyPath.Contains(".Array.data[");
            return isArrayElement;
        }

        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            bool isIsArrayElement = IsArrayElement(property);
            UnityObjectReference reference = null;
            if(isIsArrayElement)
            {
                var propertyPath = property.propertyPath;
                var paths = propertyPath.Split('.');
                object result = property.serializedObject.targetObject;
                for (int i = 0; i < paths.Length; i++)
                {
                    var path = paths[i];
                    if(path == "Array")
                    {
                        i += 1;
                        var indexPath = paths[i];
                        var start = indexPath.LastIndexOf('[') + 1;
                        var len = indexPath.LastIndexOf(']') - start;
                        var num = indexPath.Substring(start, len);
                        var index = int.Parse(num);
                        var objs = result as IEnumerable<object>;
                        foreach (var o in objs)
                        {
                            if(index == 0)
                            {
                                result = o;
                                break;
                            }
                            index --;
                        }
                    }
                    else
                    {
                        var filed = result.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        result = filed.GetValue(result);
                    }
                }
                reference = result as UnityObjectReference;
            }
            else
            {
                reference = this.fieldInfo.GetValue(property.serializedObject.targetObject) as UnityObjectReference;
            }
            if (reference != null)
            {
                //EditorGUI.PropertyField(position, property);
                var uobj = reference.GetDisplayObject();
                var type = reference.GetObjectType();
                var newObj = EditorGUI.ObjectField(position, property.displayName, uobj, type, true);
                bool isUpdate = reference.UpdateUnityObject(newObj);
                var owner = reference.GetOwner();
                if (isUpdate)
                {
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
            
        }
    }
}
