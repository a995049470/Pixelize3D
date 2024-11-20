using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{

    //防御力组件
    [DefaultEntityComponentProcessor(typeof(CalculateDamageProcessor))]
    [DefaultEntityComponentProcessor(typeof(DefenseClearProcessor))]
    public class DefenseComponent : EntityComponent
    {
        public DefenseReceiver EmptyHandDamageDefense = new();
        public DefenseReceiver PhysicalDamageDefense = new();

        public void Clear()
        {
            EmptyHandDamageDefense.Claer();
            PhysicalDamageDefense.Claer();
        }
        
    }
}
