using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    

    public class EatProcessor : SimpleGameEntityProcessor<EatComponent, ActionMaskComponent, HurtComponent, RotateComponent>
    {
        public EatProcessor() : base()
        {
            Order = ProcessorOrder.Eat;
        }

        public override void Update(GameTime time)
        {
            foreach (var kvp in ComponentDatas)
            {
                var eatComp = kvp.Value.Component1;
                var actionMaskComp = kvp.Value.Component2;
                if(eatComp.IsEatting)
                {
                    bool isEatUpdate = true;
                    bool isInvokeEat = false;
                    var rotateComp = kvp.Value.Component4;
                    if (!actionMaskComp.IsActionEnable(ActionFlag.Eat) ||
                       rotateComp.FaceDirection != eatComp.EatDir)
                    {
                        eatComp.ForceCompleteEat();
                        isEatUpdate = false;
                    }
                    if (!DirectionUtil.IsCorrectFaceDir(eatComp.Entity.Transform.Forward, eatComp.EatDir))
                    {
                        isEatUpdate = false;
                    }
                    if (isEatUpdate)
                    {
                        isInvokeEat = eatComp.TryEat(time.DeltaTime);
                    }
                    if(isInvokeEat)
                    {
                        var hurtComp = kvp.Value.Component3;
                        var value = time.DeltaTime / eatComp.Duration * eatComp.TotalRecover ;
                        hurtComp.Heal += value;
                    }
                }
            }
        }

    }
}
