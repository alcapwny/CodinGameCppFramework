using System;

namespace CGFCodeGenerator.Core
{
    public class CGFAttributeData
    {

        public static CGFAttributeData Parse(CGFParserReporter reporter, Microsoft.CodeAnalysis.AttributeData attributeData)
        {
            //TODO - Add way to have custom validation for ArrayAttribute to validate variable name actually matches an serialized variable
            Type attributeType = Type.GetType(attributeData.AttributeClass.ToString(), false);
            Type baseAttributeType = typeof(CGFAttribute);
            if (!attributeType.IsSubclassOf(baseAttributeType))
            {
                reporter.LogInfo($"Unrecognized attribute type {attributeType.Name}. Attributes are expected to be subtypes of {baseAttributeType.Name}.");
                return null;
            }

            Attribute attribute = null;
            using (reporter.CreateContextScope(CGFParserReporterContext.Type.Type, attributeType.Name))
            {
                System.Reflection.ConstructorInfo[] constructors = attributeType.GetConstructors();
                foreach (System.Reflection.ConstructorInfo constructorInfo in constructors)
                {
                    System.Reflection.ParameterInfo[] parameters = constructorInfo.GetParameters();
                    if (attributeData.ConstructorArguments.Length != parameters.Length)
                        continue;

                    bool matchingParameters = true;
                    for (int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex)
                    {
                        Microsoft.CodeAnalysis.TypedConstant cgfTypedConstant = attributeData.ConstructorArguments[paramIndex];
                        System.Reflection.ParameterInfo parameterInfo = parameters[paramIndex];

                        Type cgfParameterType = null;
                        if (string.IsNullOrEmpty(cgfTypedConstant.Type.Name))
                            cgfParameterType = Type.GetType(cgfTypedConstant.Type.Name, false);
                        else
                            cgfParameterType = Type.GetType($"{cgfTypedConstant.Type.ContainingSymbol.Name}.{cgfTypedConstant.Type.Name}", false);

                        if (cgfParameterType != parameterInfo.ParameterType)
                        {
                            matchingParameters = false;
                            break;
                        }
                    }

                    if (matchingParameters)
                    {
                        if (parameters.Length > 0)
                        {
                            Object[] parameterObjects = new Object[parameters.Length];
                            for (int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex)
                            {
                                Microsoft.CodeAnalysis.TypedConstant cgfTypedConstant = attributeData.ConstructorArguments[paramIndex];
                                System.Reflection.ParameterInfo parameterInfo = parameters[paramIndex];

                                Object convertedParam = null;
                                if (parameterInfo.ParameterType.IsEnum)
                                {
                                    convertedParam = Enum.ToObject(parameterInfo.ParameterType, cgfTypedConstant.Value);
                                }
                                else
                                {
                                    convertedParam = System.Convert.ChangeType(cgfTypedConstant.Value, parameterInfo.ParameterType);
                                }

                                parameterObjects[paramIndex] = convertedParam;
                            }

                            attribute = Activator.CreateInstance(attributeType, parameterObjects) as Attribute;
                        }
                        else
                        {
                            attribute = Activator.CreateInstance(attributeType) as Attribute;
                        }
                    }
                }

                if (attribute == null)
                {
                    //TODO - Add provided constructor param list to help finding the error.
                    //       List most likely constructors.
                    reporter.LogError($"No matching constructor found for {attributeType.Name}.");
                    return null;
                }

                CGFAttributeData CGFAttributeData = new CGFAttributeData(attribute, attributeData);
                return CGFAttributeData;
            }
        }

        static bool IsAttributeType(string name, Type attributeType)
        {
            if (name == attributeType.Name)
                return true;

            int attributeNameLength = attributeType.Name.Length - "Attribute".Length;
            if (attributeNameLength > 0)
            {
                if (name == attributeType.Name.Substring(0, attributeNameLength))
                    return true;
            }

            return false;
        }

        CGFAttributeData(Attribute attribute, Microsoft.CodeAnalysis.AttributeData attributeData)
        {
            Attribute = attribute;
            m_AttributeData = attributeData;
        }

        public string Name { get { return m_AttributeData.ToString(); } }

        public Attribute Attribute { get; private set; }
        Microsoft.CodeAnalysis.AttributeData m_AttributeData;
    }
}
