using System;

namespace CGFCodeGenerator
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class VariableSizeArrayAttribute : Core.CGFAttribute
    {
        public VariableSizeArrayAttribute(string arraySizeFieldName)
        {
            ArraySizeFieldName = arraySizeFieldName;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + ArraySizeFieldName + ")";
        }

        public string ArraySizeFieldName { get; }
    }
}
