using System;
using System.Collections.Generic;

namespace CGFCodeGenerator
{
    public class ArrayDimension
    {
        public static implicit operator ArrayDimension(string s) => new ArrayDimension(s);
        public static implicit operator ArrayDimension(uint x) => new ArrayDimension(x);

        public ArrayDimension(string arraySizeFieldName)
        {
            SizeString = arraySizeFieldName;
            IsFixedSize = false;
            LibraryName = "vector";
            TypeName = "std::vector";
        }

        public ArrayDimension(uint arraySize)
        {
            SizeString = arraySize.ToString();
            IsFixedSize = true;
            LibraryName = "array";
            TypeName = "std::array";
        }

        public override string ToString()
        {
            return SizeString;
        }

        public string SizeString { get; }
        public bool IsFixedSize { get; }
        public bool IsVariableSize { get { return !IsFixedSize; } }
        public string LibraryName { get; }
        public string TypeName { get; }
    }


    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class ArrayAttribute : Core.CGFAttribute
    {
        public ArrayAttribute(string arraySizeFieldName)
        {
            Dimensions.Add(arraySizeFieldName);
        }

        public ArrayAttribute(string arrayXSizeFieldName, string arrayYSizeFieldName)
        {
            Dimensions.Add(arrayXSizeFieldName);
            Dimensions.Add(arrayYSizeFieldName);
        }

        public ArrayAttribute(uint arraySize)
        {
            Dimensions.Add(arraySize);
        }

        public ArrayAttribute(uint arrayXSize, uint arrayYSize)
        {
            Dimensions.Add(arrayXSize);
            Dimensions.Add(arrayYSize);
        }

        public ArrayAttribute(uint arrayXSize, string arrayYSizeFieldName)
        {
            Dimensions.Add(arrayXSize);
            Dimensions.Add(arrayYSizeFieldName);
        }

        public ArrayAttribute(string arrayXSizeFieldName, uint arrayYSize)
        {
            Dimensions.Add(arrayXSizeFieldName);
            Dimensions.Add(arrayYSize);
        }

        public override string ToString()
        {
            string returnString = base.ToString() + "(";

            for (int i = 0; i < Dimensions.Count; ++i)
            {
                returnString += Dimensions[i];
                if (i != (Dimensions.Count - 1))
                {
                    returnString += ", ";
                }
            }

            returnString += ")";

            return returnString;
        }

        public bool HasVariableSizeDimension()
        {
            foreach (ArrayDimension dimension in Dimensions)
            {
                if (dimension.IsVariableSize)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasFixedSizeDimension()
        {
            foreach (ArrayDimension dimension in Dimensions)
            {
                if (dimension.IsFixedSize)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ArrayDimension> Dimensions { get; } = new List<ArrayDimension>();
    }
}
