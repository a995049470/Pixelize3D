using UnityEngine;
using Lost.Runtime.Footstone.Core;
using Lost.Render.Runtime;

namespace Lost.Runtime.Footstone.Game
{
    public class HurtFlashProcessor : SimpleGameEntityProcessor<RendererComponent, HurtEffectComponent>
    {
        private MaterialPropertyBlock materialPropertyBlock;

        public HurtFlashProcessor() : base()
        {
            Order = ProcessorOrder.ExecuteHurtEffect;
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        //shader里那个smoothStep
        private float SmoothStep(float edge0, float edge1, float x)
        {
            float t = Mathf.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            return t * t * (3.0f - 2.0f * t);
        }        

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var effect = kvp.Value.Component2;
                var rendererComp = kvp.Value.Component1;
                
                if(effect.IsFlashStateChange)
                {
                    //renderer.GetPropertyBlock(materialPropertyBlock);
                    if(!effect.IsFlash)
                        materialPropertyBlock.SetColor(ShaderConstant._HitColor, Color.black);
                    rendererComp.SetPropertyBlock(materialPropertyBlock);    
                }
                else if(effect.IsFlash)
                {
                    
                    float p = effect.FlashTimer.x /  effect.FlashTimer.y;
                    p = SmoothStep(0.0f, 0.25f, p) * SmoothStep(1.0f, 0.75f, p);
                    Color color = Color.Lerp(Color.black, effect.FlashColor, p);
                    materialPropertyBlock.SetColor(ShaderConstant._HitColor, color);
                    rendererComp.SetPropertyBlock(materialPropertyBlock);    
                }
                
            }
        }
    }
}
