using System;

namespace CGFCodeGenerator
{
    [FlagsAttribute]
    public enum SerializeFlags
    {
        Callback, //Used when custom serialization code is needed for the type
        CallbackFullLine, //Used when custom serialization code is needed for the type AND the type is responsible for parsing/outputting a full line
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
    public class SerializeAttribute : Core.CGFAttribute
    {
        public SerializeAttribute(SerializeFlags flags) { Flags = flags; }
        public SerializeFlags Flags { get; }
    }   
}
