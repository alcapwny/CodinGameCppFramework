using System;
using System.Collections.Generic;

namespace CGFCodeGenerator.Core
{
    public class CGFParserReporter
    {
        public ContextScope CreateContextScope(CGFParserReporterContext.Type contextType, String filePath)
        {
            CGFParserReporterContext reporterContext = CGFParserReporterContext.CreateContext(contextType, filePath);
            ContextScope contextScope = new ContextScope(this, reporterContext);
            return contextScope;
        }

        public class ContextScope : IDisposable
        {
            public ContextScope(CGFParserReporter reporter, CGFParserReporterContext reporterContext)
            {
                m_Reporter = reporter;
                m_Reporter.PushContext(reporterContext);
            }

            void IDisposable.Dispose()
            {
                m_Reporter.PopContext();
            }

            CGFParserReporter m_Reporter;
        }

        public void LogInfo(string message)
        {
            LogReport(CGFParserReportDetails.Level.Info, message);
        }

        public void LogDebug(string message)
        {
            LogReport(CGFParserReportDetails.Level.Debug, message);
        }

        public void LogWarning(string message)
        {
            LogReport(CGFParserReportDetails.Level.Warning, message);
        }

        public void LogError(string message)
        {
            LogReport(CGFParserReportDetails.Level.Error, message);
        }

        public bool HasError()
        {
            return m_ReportDetailsLevelCount[(int)CGFParserReportDetails.Level.Error] > 0;
        }

        void LogReport(CGFParserReportDetails.Level level, string message)
        {
            CGFParserReportDetails reportDetails = new CGFParserReportDetails();
            reportDetails.ErrorLevel = level;
            reportDetails.ErrorMessage = message;
            LogReport(reportDetails);
        }

        void LogReport(CGFParserReportDetails reportDetails)
        {
            int depth = 0;
            const int indentationPerDepth = 2;
            if (m_ReporterContextStack.Count > 0)
            {
                CGFParserReporterContext[] contextList = m_ReporterContextStack.ToArray();
                for(int contextIndex = contextList.Length - 1; contextIndex >= 0; --contextIndex)
                {
                    CGFParserReporterContext context = contextList[contextIndex];
                    Console.WriteLine($"[{ reportDetails.ErrorLevel.ToString() }]" + "".PadLeft(depth + 1) + context.ContextName);
                    depth += indentationPerDepth;
                }
            }

            Console.WriteLine($"[{ reportDetails.ErrorLevel.ToString() }]" + "".PadLeft(depth + 1) + $"{ reportDetails.ErrorMessage }".PadLeft(depth));

            m_ReportDetailsLevelCount[(int)reportDetails.ErrorLevel]++;
        }

        void PushContext(CGFParserReporterContext reporterContext)
        {
            if (m_ReporterContextStack.Count != 0)
            {
                CGFParserReporterContext top = m_ReporterContextStack.Peek();
                if (top != null)
                {
                    reporterContext.ParentContext = top;
                }
            }

            m_ReporterContextStack.Push(reporterContext);
        }
        
        void PopContext()
        {
            m_ReporterContextStack.Pop();
        }

        Stack<CGFParserReporterContext> m_ReporterContextStack = new Stack<CGFParserReporterContext>();
        int[] m_ReportDetailsLevelCount = new int[(int)CGFParserReportDetails.Level.Count];
    }
}
