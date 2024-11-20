using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lost.Runtime.Footstone.Game;
using UnityEditor.Animations;
using System.Reflection;

namespace Lost.Editor.Footstone
{

    [CustomEditor(typeof(PoseComponent))]
    public class PoseComponentEditor : UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("UpdateClipInfoList"))
            {
                UpdateClipInfoList(target as PoseComponent);
            }
        }
        
        private void UpdateClipInfoList(PoseComponent pose)
        {
            var animator = pose.Component as Animator;
            var controller = animator.runtimeAnimatorController as AnimatorController;
            var type = pose.GetType();
            var flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var filed_cacheClipInfos = type.GetField("cacheClipInfos", flag);
            var originInfos = filed_cacheClipInfos?.GetValue(pose) as List<ClipInfo>;
            var cacheClipInfos = new List<ClipInfo>();
            foreach (var layer in controller.layers)
            {
                var stateMachine = layer.stateMachine;
                foreach (var state in stateMachine.states)
                {
                    var stateName = state.state.name;
                    var flagName = stateName.Split('_')[0];
                    var stateDuration = state.state.motion != null ? state.state.motion.averageDuration : 0;
                    bool isSuccess = System.Enum.TryParse<StateFlag>(flagName, out var result);
                    if(isSuccess)
                    {
                        ClipInfo clipInfo = originInfos.Find(x => x.Name == stateName);
                        if(clipInfo == null)
                        {
                            clipInfo = new ClipInfo();
                            clipInfo.Name = stateName;
                            clipInfo.Flag = result;
                            clipInfo.Duration = stateDuration;
                        }
                        cacheClipInfos.Add(clipInfo);
                    }

                }
            }
            filed_cacheClipInfos?.SetValue(pose, cacheClipInfos);
            EditorUtility.SetDirty(pose);
        }
    }
}
