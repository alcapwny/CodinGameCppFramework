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
                case "Int64":
                    return "long long int";
                case "UInt64":
                    return "unsigned long long int";
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
