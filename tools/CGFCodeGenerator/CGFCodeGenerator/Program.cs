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
        static string UserFileHeaderTag = "/////////////////////////////////////////////////////////////////////////////////////";
        static string UserFileHashTag = "///Hash:";

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
			            CGFAttributeDataList fieldAttributes = field.Attributes;
                        ArrayAttribute arrayAttribute = fieldAttributes.GetAttribute<ArrayAttribute>();
                        if (arrayAttribute != null)
                        {
                            foreach(ArrayDimension dimension in arrayAttribute.Dimensions)
                            {
                                systemIncludes.Add(dimension.LibraryName);
                            }
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
                WriteUserFile(userPath, userCode, reporter);
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
                string previousFileContents = File.ReadAllText(filepath);
                if (previousFileContents != fileContents)
                {
                    File.WriteAllText(filepath, fileContents);
                }
            }
        }

        static void WriteUserFile(string filepath, string fileContents, CGFParserReporter reporter)
        {
            if (!File.Exists(filepath))
            {
                string directory = Path.GetDirectoryName(filepath);
                Directory.CreateDirectory(directory);

                string newFileContents = GenerateUserFileWithHash(filepath, fileContents);
                File.WriteAllText(filepath, newFileContents);
            }
            else
            {
                //Get previous file hash
                string[] existingFileStrings = File.ReadAllLines(filepath, Encoding.UTF8);

                if (existingFileStrings.Length < 2)
                {
                    reporter.LogWarning(filepath + " has unexpected content. This file will not be modified. To have it automatically regenerated delete the file and rerun the code generator.");
                    return;
                }

                if (!existingFileStrings[1].StartsWith(UserFileHashTag))
                {
                    reporter.LogWarning(filepath + " has unexpected content. This file will not be modified. To have it automatically regenerated delete the file and rerun the code generator.");
                    return;
                }

                string existingFileHash = existingFileStrings[1].Substring(UserFileHashTag.Length);

                //Calculate previous file hash
                StringBuilder existingFileNoHashBuilder = new StringBuilder();
                for (int i = 2; i < existingFileStrings.Length; i++)
                {
                    existingFileNoHashBuilder.Append(existingFileStrings[i]);
                    existingFileNoHashBuilder.Append(System.Environment.NewLine);
                }
                string existingFileContentsNoHash = existingFileNoHashBuilder.ToString();
                string calculatedHash = ComputeHashString(existingFileContentsNoHash);

                if (calculatedHash != existingFileHash)
                {
                    reporter.LogInfo(filepath + " has a hash that does not match the calculated file hash. This likely means it was modified by hand. This file will not be modified. To have it automatically regenerated delete the file and rerun the code generator.");
                    return;
                }

                //Create one string with the contents of the existingFile
                StringBuilder existingFileBuilder = new StringBuilder();
                for (int i = 0; i < existingFileStrings.Length; i++)
                {
                    existingFileBuilder.Append(existingFileStrings[i]);
                    existingFileBuilder.Append(System.Environment.NewLine);
                }
                string existingFileContents = existingFileBuilder.ToString();

                //Create string for new contents
                string newFileContents = GenerateUserFileWithHash(filepath, fileContents);

                //Only write file if contents are different
                if (existingFileContents != newFileContents)
                {
                    File.WriteAllText(filepath, newFileContents);
                }
            }
        }

        static string ComputeHashString(string content)
        {
            System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] hashBytes = sha256.ComputeHash(contentBytes);

            // Convert byte array to a string   
            StringBuilder hashBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashBuilder.Append(hashBytes[i].ToString("x2"));
            }
            string hashString = hashBuilder.ToString();
            return hashString;
        }

        static string GenerateUserFileWithHash(string filepath, string fileContents)
        {
            string hashString = ComputeHashString(fileContents);

            StringBuilder fileContentsBuilder = new StringBuilder();
            fileContentsBuilder.Append(UserFileHeaderTag);
            fileContentsBuilder.Append(System.Environment.NewLine);

            fileContentsBuilder.Append(UserFileHashTag);
            fileContentsBuilder.Append(hashString);
            fileContentsBuilder.Append(System.Environment.NewLine);

            fileContentsBuilder.Append(fileContents);

            return fileContentsBuilder.ToString();
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
