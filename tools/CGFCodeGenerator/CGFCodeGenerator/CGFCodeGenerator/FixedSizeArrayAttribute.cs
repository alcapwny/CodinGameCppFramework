using System;

namespace CGFCodeGenerator
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class FixedSizeArrayAttribute : Core.CGFAttribute
    {
        public FixedSizeArrayAttribute(uint arraySize)
        {
            ArraySize = arraySize;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + ArraySize + ")";
        }

        public uint ArraySize { get; }
    }
}
