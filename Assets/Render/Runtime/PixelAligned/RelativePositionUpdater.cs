using System.Collections;
using System.Collections.Generic;
using Lost.Render.Runtime;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class RelativePositionUpdater : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Renderer selfRenderer;
    private static MaterialPropertyBlock _materialPropertyBlock;
    private static MaterialPropertyBlock materialPropertyBlock
    {
        get
        {
            if(_materialPropertyBlock == null) _materialPropertyBlock = new MaterialPropertyBlock();
            return _materialPropertyBlock;
        }
    }
    
    private void OnEnable() {
        selfRenderer = this.GetComponent<Renderer>();
    }

    private void UpdateRelativePosition()
    {
        if(selfRenderer.isVisible)
        {
            var relativePos = Vector3.zero;
            if(target != null)
            {
                relativePos = target.position - this.transform.position;
            }
            materialPropertyBlock.SetVector(ShaderConstant._RelativePosition, relativePos);
            selfRenderer.SetPropertyBlock(materialPropertyBlock);
        }

    }
    
    private void OnDisable() {
        selfRenderer.SetPropertyBlock(null);
    }
    
    // private void OnPreRender() {
    //     UpdateRelativePosition();
    // }
    
    private void Update() {
        UpdateRelativePosition();
    }

}
