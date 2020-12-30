namespace CGFCodeGenerator.Core
{
    class CGFParserUtils
    {
        public static string GetCPPStringForType(string csharpType)
        {
            switch (csharpType)
            {
                case "Int32":
                    return "int";
                case "UInt32":
                    return "unsigned int";
                case "String":
                    return "std::string";
                case "Boolean":
                    return "bool";
                default:
                    return csharpType;
            }
        }
    }
}
