using System;

namespace Lost.Runtime.Footstone.Core
{
    [Flags]
    public enum ExecutionMode
    {
        None = 0,
        Runtime = 1,
        Editor = 2,
        Thumbnail = 4,
        Preview = 8,
        EffectCompile = 16,
        All = Runtime | Editor | Thumbnail | Preview | EffectCompile,
    }
} 

