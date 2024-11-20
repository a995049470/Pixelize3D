// using System.Collections;
// using System.Collections.Generic;
// using Lost.Render.Runtime;
// using UnityEngine;


// namespace Lost.Render.Runtime
// {

//     [ExecuteAlways]
//     [RequireComponent(typeof(Renderer))]
//     public class RelativePositionUpdater : MonoBehaviour
//     {
//         // [SerializeField]
//         // private Transform target;
//         // [SerializeField]
//         // private Vector3 targetObjectPosition;
//         // private Renderer selfRenderer;
//         // private Vector3 cacheRelativePos;
//         // private static MaterialPropertyBlock _materialPropertyBlock;
//         // private static MaterialPropertyBlock materialPropertyBlock
//         // {
//         //     get
//         //     {
//         //         if (_materialPropertyBlock == null) _materialPropertyBlock = new MaterialPropertyBlock();
//         //         return _materialPropertyBlock;
//         //     }
//         // }

//         // private void OnEnable()
//         // {
//         //     selfRenderer = this.GetComponent<Renderer>();
//         //     UpdateRelativePosition(true);
//         // }

//         // private void UpdateRelativePosition(bool isForce)
//         // {
//         //     if (selfRenderer.isVisible || isForce)
//         //     {
//         //         var targetPos = target != null ? target.position : targetObjectPosition;
//         //         var relativePos = targetPos - this.transform.position;
//         //         if(relativePos != cacheRelativePos || isForce)
//         //         {
//         //             cacheRelativePos = relativePos;
//         //             materialPropertyBlock.SetVector(ShaderConstant._RelativePosition, relativePos);
//         //             selfRenderer.SetPropertyBlock(materialPropertyBlock);
//         //         }
//         //     }

//         // }

//         // private void OnDisable()
//         // {
//         //     selfRenderer.SetPropertyBlock(null);
//         // }

//         // // private void OnPreRender()
//         // // {
//         // //     UpdateRelativePosition();
//         // // }

//         // private void Update() {
//         //     UpdateRelativePosition(false);
//         // }

//     }

// }
