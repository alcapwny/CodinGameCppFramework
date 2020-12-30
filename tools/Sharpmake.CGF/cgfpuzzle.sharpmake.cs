using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]
[module: Sharpmake.Include("cgfcsharpproject.sharpmake.cs")]

[Generate]
public class CGFPuzzleProject : CGFProject
{
    public static string RootFolderName = "codingamePuzzles";

    public CGFPuzzleProject(string projectName, string folderName)
    {
        AddTargets(CGFTargets.GetCommonTargets());

        Name = projectName;
        FolderName = RootFolderName + @"\" + folderName;
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "Puzzles";

        conf.AddPublicDependency<CGFFrameworkProject>(target);

        if (target.Optimization != Optimization.Retail)
        {
            //Define to indicate that we are compiling for local debugging
            conf.Defines.Add("CODING_LOCALDEBUGGING");
        }
        else
        {
            //Define so that system includes are ignored by the preprocessor
            conf.AdditionalCompilerOptions.Add(@"/DCODING_EXLUDESYSTEMHEADERS=");

            conf.Options.Add(Options.Vc.Compiler.GenerateProcessorFile.WithoutLineNumbers);
            conf.Options.Add(Options.Vc.Compiler.MultiProcessorCompilation.Disable);
        }
    }
}

[Generate]
public class CGFPuzzleCSharpProject : CGFCSharpProject
{
    public static string RootFolderName = "codingamePuzzles";

    public CGFPuzzleCSharpProject(string projectName, string folderName)
    {
        Name = projectName + "CSharp";
        FolderName = RootFolderName + @"\" + folderName;
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.DotNetClassLibrary;

        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "Puzzles";

        conf.ReferencesByPath.Add(@"[conf.ProjectPath]\..\..\..\bin\CGFCodeGenerator\CGFCodeGenerator.exe");

        conf.AddPublicDependency<CGFFrameworkCSharpProject>(target);
    }
}
