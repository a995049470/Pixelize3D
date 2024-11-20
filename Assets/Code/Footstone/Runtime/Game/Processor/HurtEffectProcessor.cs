using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    public class HurtEffectProcessor : SimpleGameEntityProcessor<HurtEffectComponent, HurtComponent>
    {
        public HurtEffectProcessor() : base()
        {
            Order = ProcessorOrder.UpdateHurtComponent;
        }
        public override void Update(GameTime time)
        {
            var cmd = commandBufferManager.Get();
            foreach (var kvp in ComponentDatas)
            {
                var effect = kvp.Value.Component1;
                var hurt = kvp.Value.Component2;
                var isFlashStateChange = false;
                if(hurt.IsReceiveDamage)
                {
                    isFlashStateChange = !effect.IsFlash;
                    effect.OnReceiveDamage();
                    var hitDirs = hurt.HitDirs;
                    if(!string.IsNullOrEmpty(effect.ParticletName) && hitDirs.Count > 0)
                    {
                        for (int i = 0; i < hitDirs.Count; i++)
                        {
                            var hitDir = hitDirs[i];
                            var pos = effect.Entity.Transform.Position + hitDir * 0.2f;
                            cmd.InstantiateParticle(effect.ParticletName, pos);
                        }
                    }
                }
                else if(effect.IsFlash)
                {
                    effect.FlashTimer.x += time.DeltaTime;
                    isFlashStateChange = !effect.IsFlash;
                }
                effect.IsFlashStateChange = isFlashStateChange;
            }
            cmd.Execute();
            commandBufferManager.Release(cmd);
        }
    }
}
