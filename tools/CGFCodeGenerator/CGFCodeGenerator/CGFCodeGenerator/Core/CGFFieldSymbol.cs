namespace CGFCodeGenerator.Core
{
    public class CGFFieldSymbol
    {
        public static CGFFieldSymbol Parse(CGFParserReporter reporter, Microsoft.CodeAnalysis.IFieldSymbol fieldSymbol)
        {
            CGFFieldSymbol cgfFieldSymbol = new CGFFieldSymbol(fieldSymbol);

            cgfFieldSymbol.m_AttributeDataList = CGFAttributeDataList.Parse(reporter, fieldSymbol.GetAttributes());

            return cgfFieldSymbol;
        }

        CGFFieldSymbol(Microsoft.CodeAnalysis.IFieldSymbol fieldSymbol)
        {
            m_FieldSymbol = fieldSymbol;

            Microsoft.CodeAnalysis.ITypeSymbol typeSymbol = m_FieldSymbol.Type;
            if (typeSymbol.TypeKind == Microsoft.CodeAnalysis.TypeKind.Array)
            {
                Microsoft.CodeAnalysis.IArrayTypeSymbol arrayTypeSymbol = fieldSymbol.Type as Microsoft.CodeAnalysis.IArrayTypeSymbol;
                TypeName = arrayTypeSymbol.ElementType.Name + "[]";
                UnderlyingSpecialType = arrayTypeSymbol.ElementType.SpecialType;
                IsSystemType = arrayTypeSymbol.ElementType.SpecialType != Microsoft.CodeAnalysis.SpecialType.None;
            }
            else
            {
                TypeName = typeSymbol.Name;
                UnderlyingSpecialType = typeSymbol.SpecialType;
                IsSystemType = typeSymbol.SpecialType != Microsoft.CodeAnalysis.SpecialType.None;
            }
        }

        public string Name { get { return m_FieldSymbol.Name; } }
        public string TypeName { get; }
        public CGFTypeSymbol TypeSymbol { get; set; }
        public bool IsSystemType { get; }
        public Microsoft.CodeAnalysis.SpecialType UnderlyingSpecialType { get; }
        public bool HasConstantValue { get { return m_FieldSymbol.HasConstantValue; } }
        public object ConstantValue { get { return m_FieldSymbol.ConstantValue; } }
        public string DeclaredAccessibility { get { return m_FieldSymbol.DeclaredAccessibility.ToString().ToLower(); } }
        public Microsoft.CodeAnalysis.Accessibility DeclaredAccessibilityType { get { return m_FieldSymbol.DeclaredAccessibility; } }

        public CGFAttributeDataList Attributes { get { return m_AttributeDataList; } }

        Microsoft.CodeAnalysis.IFieldSymbol m_FieldSymbol;
        CGFAttributeDataList m_AttributeDataList;
    }
}
