using System;

namespace CGFCodeGenerator
{
    public enum SerializeFlags
    {
        None,
        Custom, //Used when custom serialization code is needed for the type
        CustomFullLine, //Used when custom serialization code is needed for the type AND the type is responsible for parsing/outputting a full line
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class ExtendAttribute : Core.CGFAttribute
    {
        public ExtendAttribute() { Flags = SerializeFlags.None; }
        public ExtendAttribute(SerializeFlags flags) { Flags = flags; }
        
        public SerializeFlags Flags { get; }
        public bool HasCustomSerialization { get { return Flags == SerializeFlags.Custom || Flags == SerializeFlags.CustomFullLine; } }
    }
}
