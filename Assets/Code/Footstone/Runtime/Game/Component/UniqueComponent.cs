using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    //独特组件 表示触发下一次不会出现
    [DefaultEntityComponentProcessor(typeof(UniqueInteractiveTriggerProcessor))]
    public class UniqueComponent : EntityComponent
    {

    }

}
