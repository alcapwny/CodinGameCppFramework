using System;

namespace CGFCodeGenerator.Core
{
    public class CGFParserReportDetails
    {
        public enum Level
        {
            Info,
            Debug,
            Warning,
            Error,
            Count,
        }

        public Level ErrorLevel { get; set; }
        public String ErrorMessage { get; set; }
    }
}
