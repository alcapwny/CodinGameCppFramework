using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CGFCodeGenerator.Core
{
    public class CGFParser
    {
        public static CGFParser ParseSolution(CGFParserReporter reporter, string solutionPath)
        {
            string fullSolutionPath = Path.GetFullPath(solutionPath);
            reporter.LogInfo($"CGFParser parsing {solutionPath} (FullPath: {fullSolutionPath})");

            if (!File.Exists(fullSolutionPath))
            {
                reporter.LogError($"{fullSolutionPath} does not exist.");
                return null;
            }

            CGFParser cgfParser = new CGFParser(reporter);

            string[] documentsToExclude = { "AssemblyAttributes.cs" };

            reporter.LogInfo($"Opening solution {solutionPath}");

            IEnumerable<VisualStudioInstance> visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances(new VisualStudioInstanceQueryOptions { DiscoveryTypes = DiscoveryType.VisualStudioSetup });
            VisualStudioInstance visualStudioInstance = visualStudioInstances.OrderByDescending(i => i.Version)
                .Where(i => Directory.Exists(i.MSBuildPath))
                .FirstOrDefault();

            if (string.IsNullOrEmpty(visualStudioInstance.MSBuildPath))
            {
                reporter.LogError($"Unable to find a proper VisualStudio instance. MSBuild workspace creation/solution parsing may fail");
            }
            else
            {
                MSBuildLocator.RegisterInstance(visualStudioInstance);
            }

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            workspace.LoadMetadataForReferencedProjects = false;
            workspace.WorkspaceFailed += cgfParser.Workspace_WorkspaceFailed;
            workspace.SkipUnrecognizedProjects = true;
            Microsoft.CodeAnalysis.Solution solution = workspace.OpenSolutionAsync(solutionPath).Result;

            reporter.LogInfo($"Solution opened");

            using (reporter.CreateContextScope(CGFParserReporterContext.Type.Solution, solution.FilePath))
            {
                reporter.LogInfo($"Parsing");
                List<CGFDocument> cgfDocuments = new List<CGFDocument>();
                foreach (Microsoft.CodeAnalysis.Project project in solution.Projects)
                {
                    List<Microsoft.CodeAnalysis.Document> documentsToProcess = new List<Microsoft.CodeAnalysis.Document>();

                    foreach (Microsoft.CodeAnalysis.Document document in project.Documents)
                    {
                        bool includeDocument = true;
                        foreach (string documentToExclude in documentsToExclude)
                        {
                            if (document.FilePath.EndsWith(documentToExclude, StringComparison.CurrentCultureIgnoreCase))
                            {
                                includeDocument = false;
                                break;
                            }
                        }

                        if (includeDocument)
                            documentsToProcess.Add(document);
                    }

                    cgfDocuments.AddRange(ProcessDocuments(reporter, documentsToProcess, project));
                }

                reporter.LogInfo($"Parsing complete");

                Dictionary<string, string> typeNameToPathMap = new Dictionary<string, string>();
                Dictionary<string, CGFTypeSymbol> typeNameToTypeMap = new Dictionary<string, CGFTypeSymbol>();
                foreach (CGFDocument cgfDocument in cgfDocuments)
                {
                    using (reporter.CreateContextScope(CGFParserReporterContext.Type.File, cgfDocument.DocumentFilePath))
                    {
                        foreach (CGFTypeSymbol cgfType in cgfDocument.Types)
                        {
                            using (reporter.CreateContextScope(CGFParserReporterContext.Type.Type, cgfType.Name))
                            {
                                if (typeNameToPathMap.ContainsKey(cgfType.Name))
                                {
                                    reporter.LogError($"Duplicate type {cgfType.Name} found. Other instance found in {typeNameToPathMap[cgfType.Name]}");
                                }
                                else
                                {
                                    typeNameToPathMap.Add(cgfType.Name, cgfDocument.DocumentFilePath);
                                    typeNameToTypeMap.Add(cgfType.Name, cgfType);
                                }
                            }
                        }
                    }
                }

                foreach (CGFTypeSymbol type in typeNameToTypeMap.Values)
                {
                    foreach (CGFFieldSymbol field in type.Fields)
                    {
                        CGFTypeSymbol typeSymbol = null;
                        if (typeNameToTypeMap.TryGetValue(field.TypeName, out typeSymbol))
                        {
                            field.TypeSymbol = typeSymbol;
                        }
                    }
                }

                cgfParser.m_Documents = cgfDocuments;
                cgfParser.m_TypeNameToPathMap = typeNameToPathMap;

                return cgfParser;
            }
        }

        static List<CGFDocument> ProcessDocuments(CGFParserReporter reporter, List<Microsoft.CodeAnalysis.Document> documents, Microsoft.CodeAnalysis.Project project)
        {
            using (reporter.CreateContextScope(CGFParserReporterContext.Type.Project, project.FilePath))
            {
                List<CGFDocument> documentsToProcess = new List<CGFDocument>();
                foreach (Microsoft.CodeAnalysis.Document document in documents)
                {
                    using (reporter.CreateContextScope(CGFParserReporterContext.Type.File, document.FilePath))
                    {
                        List<CGFTypeSymbol> typesToProcess = new List<CGFTypeSymbol>();

                        Microsoft.CodeAnalysis.SemanticModel semanticModel = document.GetSemanticModelAsync().Result;

                        Microsoft.CodeAnalysis.SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;
                        IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> syntaxNodes = syntaxTree.GetRoot().DescendantNodes().Where(n => (n as ClassDeclarationSyntax) != null || (n as EnumDeclarationSyntax) != null);
                        foreach (Microsoft.CodeAnalysis.SyntaxNode node in syntaxNodes)
                        {
                            ClassDeclarationSyntax classSyntax = node as ClassDeclarationSyntax;
                            if (classSyntax != null)
                            {
                                Microsoft.CodeAnalysis.INamedTypeSymbol typeSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
                                using (reporter.CreateContextScope(CGFParserReporterContext.Type.Type, typeSymbol.Name))
                                {
                                    CGFTypeSymbol cgfTypeSymbol = CGFTypeSymbol.Parse(reporter, typeSymbol);
                                    typesToProcess.Add(cgfTypeSymbol);
                                }
                            }
                            else
                            {
                                EnumDeclarationSyntax enumSyntax = node as EnumDeclarationSyntax;
                                Microsoft.CodeAnalysis.INamedTypeSymbol typeSymbol = semanticModel.GetDeclaredSymbol(enumSyntax);

                                using (reporter.CreateContextScope(CGFParserReporterContext.Type.Type, typeSymbol.Name))
                                {
                                    CGFTypeSymbol cgfTypeSymbol = CGFTypeSymbol.Parse(reporter, typeSymbol);
                                    typesToProcess.Add(cgfTypeSymbol);
                                }
                            }
                        }

                        if (typesToProcess.Count > 0)
                        {
                            CGFDocument cgfDocument = CGFDocument.Parse(reporter, document, typesToProcess);
                            documentsToProcess.Add(cgfDocument);
                        }
                    }
                }

                return documentsToProcess;
            }
        }

        private void Workspace_WorkspaceFailed(object sender, Microsoft.CodeAnalysis.WorkspaceDiagnosticEventArgs e)
        {
            if (e.Diagnostic.Kind == Microsoft.CodeAnalysis.WorkspaceDiagnosticKind.Warning)
            {
                m_Reporter.LogWarning(e.Diagnostic.Message);
            }
            else
            {
                m_Reporter.LogError(e.Diagnostic.Message);
            }
        }

        public CGFParser(CGFParserReporter reporter)
        {
            m_Reporter = reporter;
        }

        public List<CGFDocument> Documents { get { return m_Documents; } }
        public Dictionary<string, string> TypeNameToPathMap { get { return m_TypeNameToPathMap; } }

        private List<CGFDocument> m_Documents;
        private Dictionary<string, string> m_TypeNameToPathMap;
        private CGFParserReporter m_Reporter;
    }
}
