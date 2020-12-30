using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

//
// Uses provided templates to create the skeleton of a puzzle solution.
//

namespace CGFPuzzleGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: <PuzzleName> <OutputPath> <InputFilePath> [InputFilePath]*");
                return;
            }

            string puzzleName = args[0];

            string outputPath = args[1];
            if (!outputPath.EndsWith(Path.DirectorySeparatorChar.ToString()) && !outputPath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                outputPath += Path.DirectorySeparatorChar;
            outputPath = outputPath + puzzleName + Path.DirectorySeparatorChar + "source" + Path.DirectorySeparatorChar;

            Directory.CreateDirectory(outputPath);

            string puzzleNameRegexPattern = Regex.Escape("$puzzlename$");
            string puzzleNameRegexReplacement = Regex.Escape(puzzleName);

            Encoding encoding = Encoding.UTF8;

            for (int i = 2; i < args.Length; ++i)
            {
                string inputFilePath = args[i];

                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine("InputFile {0} open failed.", inputFilePath);
                    continue;
                }

                string outputFileNameFromPath = Path.GetFileName(inputFilePath);

                string outputFileName = Regex.Replace(
                    outputFileNameFromPath,
                    puzzleNameRegexPattern,
                    puzzleNameRegexReplacement,
                    RegexOptions.IgnoreCase);

                string outputFilePath = outputPath + outputFileName;
                FileStream outputFile = File.Open(outputFilePath, FileMode.Create);
                if (outputFile == null)
                {
                    Console.WriteLine("OutputFile {0} create failed.", outputFileName);
                }

                foreach(string inputFileLine in File.ReadLines(inputFilePath))
                {
                    string outputFileLine = Regex.Replace(
                        inputFileLine + Environment.NewLine,
                        puzzleNameRegexPattern,
                        puzzleNameRegexReplacement,
                        RegexOptions.IgnoreCase);

                    outputFile.Write(encoding.GetBytes(outputFileLine), 0, encoding.GetByteCount(outputFileLine));
                }

                Console.WriteLine("OutputFile {0} successfully written to.", outputFilePath);
            }
        }
    }
}
