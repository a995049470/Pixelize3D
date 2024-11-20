using UnityEngine;


namespace Lost.Render.Runtime
{
    [ExecuteAlways]
    public class ObjectPositionController : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] renderers = new Renderer[0];
        private static MaterialPropertyBlock _materialPropertyBlock;
        private static MaterialPropertyBlock materialPropertyBlock
        {
            get
            {
                if (_materialPropertyBlock == null) _materialPropertyBlock = new MaterialPropertyBlock();
                return _materialPropertyBlock;
            }
        }

        private Vector3 cachePosition;
        
        private void OnEnable() {
            UpdateObjectPosition(true);
        }

        public void FindRenderers()
        {
            renderers = this.GetComponentsInChildren<Renderer>(true);
        }

        private void Update() {
            UpdateObjectPosition(false);
        }

        public void UpdateObjectPosition(bool isForce)
        {
            if(isForce || cachePosition != this.transform.position)
            {
                cachePosition = this.transform.position;
                foreach (var renderer in renderers)
                {
                    renderer.GetPropertyBlock(materialPropertyBlock);
                    materialPropertyBlock.SetVector(ShaderConstant._ObjectPosition, cachePosition);
                    renderer.SetPropertyBlock(materialPropertyBlock);
                }
            }
        }
          
    }

}
