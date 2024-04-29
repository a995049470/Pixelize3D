using UnityEngine;


namespace Lost.Render.Runtime
{
    [System.Flags]
    public enum PassName : int
    {
        SRPDefaultUnlit = 1,
        PixelOutline = 2,
        ShadowCaster = 4,
        All = int.MaxValue
    }

    [RequireComponent(typeof(Renderer))]
    public class MaterialPassController : MonoBehaviour
    {
        [SerializeField]
        private PassName enablePassNames = PassName.All;
        private Renderer _selfRenderer;
        public Renderer selfRenderer
        {
            get
            {
                if(_selfRenderer == null)
                    _selfRenderer = GetComponent<Renderer>();
                return _selfRenderer;
            }
        }
        
        private void Awake() {
            SetShaderPass();
        }

        private void OnValidate() {
            SetShaderPass();
        }

        private void SetShaderPass()
        {
            var materials = selfRenderer.sharedMaterials;
            foreach (var material in materials)
            {
                material.SetShaderPassEnabled(nameof(PassName.SRPDefaultUnlit), (PassName.SRPDefaultUnlit & enablePassNames) > 0);
                material.SetShaderPassEnabled(nameof(PassName.PixelOutline), (PassName.PixelOutline & enablePassNames) > 0);
                material.SetShaderPassEnabled(nameof(PassName.ShadowCaster), (PassName.ShadowCaster & enablePassNames) > 0);
                
            }
        
        }

    }

}
