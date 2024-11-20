using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Lost.Editor.Footstone
{
    
    public class MaterialUpdater : SimpleWindow
    {
        private string oldSuffix = $"Legacy Shaders/";
        private string newSuffix = $"Lost/Legacy Shaders/";
        [SerializeField]
        private string root;

        [MenuItem("Tools/Material Updater")]
        static void Open()
        {
            DisplayWizard<MaterialUpdater>("MaterialUpdater", "");
        }

        protected override bool DrawWizardGUI()
        {
            var res = base.DrawWizardGUI();
            if(GUILayout.Button("更新材质"))
            {
                UpdateMaterials();
            }
            return res;
        }

        private void UpdateMaterials()
        {
            if(!string.IsNullOrEmpty(root)) 
            {
                var materials = FileUtility.LoadObjectsFromFloder<Material>(root, "*.mat");
                AssetDatabase.StartAssetEditing();
                foreach (var material in materials)
                {
                    UpdateMaterial(material);
                }
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }
        }
        
        private void UpdateMaterial(Material material)
        {
            var shaderName = material.shader.name;
            if(!string.IsNullOrEmpty(shaderName) && shaderName.StartsWith(oldSuffix))
            {
                shaderName = shaderName.Replace(oldSuffix, newSuffix);
                var targetShader = Shader.Find(shaderName);
                if(targetShader != null) material.shader = targetShader;
                
            }
        }

    }
}

