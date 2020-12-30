using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace CGFCodeGenerator.Core
{
    public class CGFTypeSymbol
    {
        public static CGFTypeSymbol Parse(CGFParserReporter reporter, INamedTypeSymbol typeSymbol)
        {
            CGFTypeSymbol cgfTypeSymbol = new CGFTypeSymbol(typeSymbol);

            foreach (ISymbol member in typeSymbol.GetMembers())
            {
                if (member.Kind == SymbolKind.Field)
                {
                    IFieldSymbol fieldMember = member as IFieldSymbol;
                    CGFFieldSymbol cgfFieldSymbol = CGFFieldSymbol.Parse(reporter, fieldMember);
                    cgfTypeSymbol.Fields.Add(cgfFieldSymbol);
                }
            }

            cgfTypeSymbol.Fields.Sort((leftField, rightField) => leftField.DeclaredAccessibilityType - rightField.DeclaredAccessibilityType);

            foreach (CGFFieldSymbol cgfFieldSymbol in cgfTypeSymbol.Fields)
            {
                switch (cgfFieldSymbol.DeclaredAccessibilityType)
                {
                    case Accessibility.Public:
                        {
                            cgfTypeSymbol.PublicFields.Add(cgfFieldSymbol);
                            break;
                        }
                    case Accessibility.Protected:
                        {
                            cgfTypeSymbol.ProtectedFields.Add(cgfFieldSymbol);
                            break;
                        }
                    case Accessibility.Private:
                        {
                            cgfTypeSymbol.PrivateFields.Add(cgfFieldSymbol);
                            break;
                        }
                }
            }

            cgfTypeSymbol.m_AttributeDataList = CGFAttributeDataList.Parse(reporter, typeSymbol.GetAttributes());

            return cgfTypeSymbol;
        }

        CGFTypeSymbol(INamedTypeSymbol typeSymbol)
        {
            m_TypeSymbol = typeSymbol;
            VariableName = Char.ToLowerInvariant(Name[0]) + Name.Substring(1);

            Fields = new List<CGFFieldSymbol>();
            PublicFields = new List<CGFFieldSymbol>();
            ProtectedFields = new List<CGFFieldSymbol>();
            PrivateFields = new List<CGFFieldSymbol>();
        }

        public string Name { get { return m_TypeSymbol.Name; } }
        public string VariableName { get; }
        public bool IsEnum { get { return m_TypeSymbol.EnumUnderlyingType != null; } }

        public CGFAttributeDataList Attributes { get { return m_AttributeDataList; } }
        public List<CGFFieldSymbol> Fields { get; }
        public List<CGFFieldSymbol> PublicFields { get; }
        public List<CGFFieldSymbol> ProtectedFields { get; }
        public List<CGFFieldSymbol> PrivateFields { get; }

        INamedTypeSymbol m_TypeSymbol;
        CGFAttributeDataList m_AttributeDataList;
    }
}
