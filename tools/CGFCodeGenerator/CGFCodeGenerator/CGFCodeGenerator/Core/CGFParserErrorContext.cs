using System;

namespace CGFCodeGenerator.Core
{
    public class CGFParserReporterContext
    {
        public static CGFParserReporterContext CreateContext(CGFParserReporterContext.Type contextType, String contextName)
        {
            CGFParserReporterContext reporterContext = new CGFParserReporterContext();
            reporterContext.ContextType = contextType;
            reporterContext.ContextName = contextName;

            return reporterContext;
        }

        public enum Type
        {
            Solution,
            Project,
            File,
            Type,
        }

        public CGFParserReporterContext ParentContext { get; set; }
        public Type ContextType { get; set; }
        public String ContextName { get; set; }
    }
}
