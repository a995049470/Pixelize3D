using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    [DefaultEntityComponentProcessor(typeof(OneShot2DAudioProcessor))]
    [DefaultEntityComponentProcessor(typeof(OneShot2DAudioPlayProcessor))]
    public class OneShot2DAudioComponent : EntityComponent
    { 
        public List<string> OneShotAudioKeys = new();
    }


}