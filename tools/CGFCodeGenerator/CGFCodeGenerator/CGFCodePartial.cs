using System.Collections.Generic;
using CGFCodeGenerator.Core;

namespace CGFCodeGenerator
{
    partial class CGFHeaderCodeGenerator
    {
        public CGFHeaderCodeGenerator(CGFDocument cgfDocument, string relativeInlinePath, string relativeUserPath, HashSet<string> systemIncludes, HashSet<string> userIncludes)
        {
            CGFDocument = cgfDocument;
            RelativeInlinePath = relativeInlinePath;
            RelativeUserPath = relativeUserPath;
            SystemIncludes = systemIncludes;
            UserIncludes = userIncludes;
        }

        public CGFDocument CGFDocument { get; }
        public string RelativeInlinePath { get; }
        public string RelativeUserPath { get; }
        public HashSet<string> SystemIncludes { get; }
        public HashSet<string> UserIncludes { get; }
    }

    partial class CGFInlineCodeGenerator
    {
        public CGFInlineCodeGenerator(CGFDocument cgfDocument)
        {
            CGFDocument = cgfDocument;
        }

        public CGFDocument CGFDocument { get; }
    }

    partial class CGFUserCodeGenerator
    {
        public CGFUserCodeGenerator(CGFDocument cgfDocument)
        {
            CGFDocument = cgfDocument;
        }

        public CGFDocument CGFDocument { get; }
    }
}
