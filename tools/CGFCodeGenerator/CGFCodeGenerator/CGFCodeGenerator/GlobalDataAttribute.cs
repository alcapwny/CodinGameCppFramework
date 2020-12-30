using System;

namespace CGFCodeGenerator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class GlobalDataAttribute : Core.CGFAttribute
    {
    }
}
