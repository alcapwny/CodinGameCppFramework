using CGFCodeGenerator.Core;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CGFCodeGenerator
{
    // 
    // CodinGameFrameworkCodeGenerator (CGFCodeGenerator) parses C# files using Roslyn to generate C++ code for the CodinGame Framework (CGF)
    //

    class Program
    {
        enum ExitCode
        {
            Success = 0,
            GenerationError = -1,
        }

        static int Main(string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Command line: exe <solutionPath>");

            string solutionPath = args[0];

            ExitCode exitCode = ProcessSolution(solutionPath);

            if (exitCode != ExitCode.Success)
            {
                if (Debugger.IsAttached)
                {
                    Debug.Assert(false);
                }
            }

            return (int)exitCode;
        }

        static ExitCode ProcessSolution(string solutionPath)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            CGFParserReporter reporter = new CGFParserReporter();
            CGFParser solutionParser = CGFParser.ParseSolution(reporter, solutionPath);
            if (solutionParser == null)
            {
                return ExitCode.GenerationError;
            }

            reporter.LogInfo("Generating code");
            foreach (CGFDocument cgfDocument in solutionParser.Documents)
            {
                string sourcePath = cgfDocument.DocumentFilePath;

                const string sourceString = "source";
                const string generatedString = "generated";
                string generatedPath = ReplaceFolderInPath(sourcePath, sourceString, generatedString);
                bool hasResolvedGeneratedPath = !generatedPath.Equals(sourcePath);

                if (!hasResolvedGeneratedPath)
                {
                    generatedPath = Path.Combine(generatedPath, "CGFGenerated");
                    reporter.LogWarning($"Failed to find {sourceString} in {sourcePath} to replace with {generatedString}. Writing generated files to Appending {generatedPath}.");
                }

                string headerPath = generatedPath.Replace(".cs", ".h");
                string inlinePath = generatedPath.Replace(".cs", ".inl");
                string relativeInlinePath;
                if (hasResolvedGeneratedPath)
                {
                    relativeInlinePath = inlinePath.Substring(inlinePath.LastIndexOf(generatedString) + generatedString.Length + 1); //1 is for path separator
                }
                else
                {
                    relativeInlinePath = Path.GetFileName(inlinePath);
                }

                string userPath = sourcePath.Replace(".cs", ".user.inl");
                string relativeUserPath = GetRelativePath(userPath, sourceString);

                //Get System Includes
                HashSet<string> systemIncludes = new HashSet<string>();
                systemIncludes.Add("iostream"); //Always need iostream for istream/ostream

                //Get User includes
                HashSet<string> userIncludes = new HashSet<string>();
                foreach (CGFTypeSymbol type in cgfDocument.Types)
                {
                    foreach (CGFFieldSymbol field in type.Fields)
                    {
                        if (field.Attributes.GetAttribute<FixedSizeArrayAttribute>() != null)
                        {
                            systemIncludes.Add("array");
                        }

                        if (field.Attributes.GetAttribute<VariableSizeArrayAttribute>() != null)
                        {
                            systemIncludes.Add("vector");
                        }

                        if (field.IsSystemType && field.UnderlyingSpecialType == SpecialType.System_String)
                        {
                            systemIncludes.Add("string");
                        }

                        string typePath = null;
                        if (solutionParser.TypeNameToPathMap.TryGetValue(field.TypeName, out typePath))
                        {
                            if (typePath != cgfDocument.DocumentFilePath)
                            {
                                string relativeTypePath = GetRelativePath(typePath, sourceString);
                                userIncludes.Add(relativeTypePath.Replace(".cs", ".h").Replace("\\", "/"));
                            }
                        }
                    }
                }

                CGFHeaderCodeGenerator headerCodeGenerator = new CGFHeaderCodeGenerator(cgfDocument, relativeInlinePath, relativeUserPath, systemIncludes, userIncludes);
                CGFInlineCodeGenerator inlineCodeGenerator = new CGFInlineCodeGenerator(cgfDocument);
                CGFUserCodeGenerator userCodeGenerator = new CGFUserCodeGenerator(cgfDocument);

                string headerCode = headerCodeGenerator.TransformText();
                string inlineCode = inlineCodeGenerator.TransformText();
                string userCode = userCodeGenerator.TransformText();

                WriteFile(headerPath, headerCode);
                WriteFile(inlinePath, inlineCode);
                if (!File.Exists(userPath))
                {
                    //Only write the user file if it doesn't already exist as we don't want to override user code
                    WriteFile(userPath, userCode);
                }
            }

            long elapsedMS = stopwatch.ElapsedMilliseconds;
            reporter.LogInfo(String.Format("Code generation done in {0:0.0} sec", elapsedMS / 1000.0f));

            if (reporter.HasError())
            {
                reporter.LogError("Code generation failed with errors");
                return ExitCode.GenerationError;
            }
            else
            {
                reporter.LogInfo("Code generated");
                return ExitCode.Success;
            }
        }

        static void WriteFile(string filepath, string fileContents)
        {
            if (!File.Exists(filepath))
            {
                string directory = Path.GetDirectoryName(filepath);
                Directory.CreateDirectory(directory);

                File.WriteAllText(filepath, fileContents);
            }
            else
            {
                File.WriteAllText(filepath, fileContents);
            }
        }

        static public string ReplaceFolderInPath(string sourcePath, string folderToReplace, string replacingFolder)
        {
            char[] directorySeperators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
            string[] directories = sourcePath.Split(directorySeperators);

            for (int i = directories.Length - 1; i >= 0; --i)
            {
                if (string.Compare(directories[i], folderToReplace, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    directories[i] = replacingFolder;
                }
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < directories.Length; ++i)
            {
                stringBuilder.Append(directories[i]);
                if (i != (directories.Length - 1))
                {
                    stringBuilder.Append(Path.DirectorySeparatorChar);
                }
            }

            return stringBuilder.ToString();
        }

        static public string GetRelativePath(string path, string searchString)
        {
            string relativePath;
            int searchStringIndex = path.LastIndexOf(searchString);
            if (searchStringIndex != -1)
            {
                relativePath = path.Substring(searchStringIndex + searchString.Length + 1); //1 is for path separator
            }
            else
            {
                relativePath = Path.GetFileName(path);
            }

            return relativePath;
        }
    }
}
