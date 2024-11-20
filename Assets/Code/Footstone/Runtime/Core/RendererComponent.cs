using System.Collections.Generic;
using UnityEngine;

namespace Lost.Runtime.Footstone.Core
{
    
    public class RendererComponent : EntityComponent
    {
        [SerializeField]
        private ChildComponentReference<Renderer>[] rendererReferences = new ChildComponentReference<Renderer>[0];
        public void GetPropertyBlock(MaterialPropertyBlock propertyBlock)
        {
            if(rendererReferences.Length > 0)
                rendererReferences[0].Component.GetPropertyBlock(propertyBlock);
        }
        public void SetPropertyBlock(MaterialPropertyBlock propertyBlock)
        {
            foreach (var renderer in rendererReferences)
            {
                renderer.Component.SetPropertyBlock(propertyBlock);
            }
        }

        public void GetPropertyBlock(int rendererIndex, MaterialPropertyBlock propertyBlock)
        {
            rendererReferences[rendererIndex].Component.GetPropertyBlock(propertyBlock);
        }

        public void SetPropertyBlock(int rendererIndex, MaterialPropertyBlock propertyBlock)
        {
            rendererReferences[rendererIndex].Component.SetPropertyBlock(propertyBlock);
        }

        public void SetShaderMaterial(Material material)
        {
            foreach (var renderer in rendererReferences)
            {
                renderer.Component.sharedMaterial = material;
            }
        }

        public void SetSharedMaterial(int rendererIndex, int materialIndex, Material material)
        {
            var renderer = rendererReferences[rendererIndex].Component;
            var sharedMaterials = renderer.sharedMaterials;
            sharedMaterials[materialIndex] = material;
            renderer.sharedMaterials = sharedMaterials;
        }

        public void SetSortingOrder(int rendereIndex, int sortingOrder)
        {
            rendererReferences[rendereIndex].Component.sortingOrder = sortingOrder;
        }

        public override void UpdateReference()
        {
            base.UpdateReference();
            foreach (var render in rendererReferences)
            {
                render.Root = this.transform;
            }
        }
    #if UNITY_EDITOR
        private void OnValidate() {
            UpdateReference();
        }
    #endif
    }
}



