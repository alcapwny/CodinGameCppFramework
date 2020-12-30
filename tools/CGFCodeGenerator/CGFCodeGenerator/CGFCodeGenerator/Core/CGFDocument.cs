using System.Collections.Generic;

namespace CGFCodeGenerator.Core
{
    public class CGFDocument
    {
        public static CGFDocument Parse(CGFParserReporter reporter, Microsoft.CodeAnalysis.Document document, List<CGFTypeSymbol> types)
        {
            CGFDocument cgfDocument = new CGFDocument(document, types);
            return cgfDocument;
        }

        CGFDocument(Microsoft.CodeAnalysis.Document document, List<CGFTypeSymbol> types)
        {
            m_Document = document;
            Types = types;
        }

        public List<CGFTypeSymbol> Types { get; }
        public string DocumentFilePath { get { return m_Document.FilePath; } }

        Microsoft.CodeAnalysis.Document m_Document;
    }
}
