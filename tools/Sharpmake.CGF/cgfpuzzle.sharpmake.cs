using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]
[module: Sharpmake.Include("cgfcsharpproject.sharpmake.cs")]

[Generate]
public class CGFPuzzleProject : CGFProject
{
    public static string RootFolderName = "codingamePuzzles";

    public CGFPuzzleProject(string projectName, string folderName, string filterName)
    {
        AddTargets(CGFTargets.GetCommonTargets());

        Name = projectName;
        FolderName = RootFolderName + @"\" + folderName;
        FilterName = filterName;
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        base.ConfigureAll(conf, target);

        if (string.IsNullOrEmpty(FilterName))
        {
            conf.SolutionFolder = "Puzzles";
        }
        else
        {
            conf.SolutionFolder = string.Format("Puzzles/{0}", FilterName);
        }

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

    private string _filterName = "";
    public string FilterName
    {
        get { return _filterName; }
        set { SetProperty(ref _filterName, value); }
    }
}

[Generate]
public class CGFPuzzleCSharpProject : CGFCSharpProject
{
    public static string RootFolderName = "codingamePuzzles";

    public CGFPuzzleCSharpProject(string projectName, string folderName, string filterName)
    {
        Name = projectName + "CSharp";
        FolderName = RootFolderName + @"\" + folderName;
        FilterName = filterName;
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.DotNetClassLibrary;

        base.ConfigureAll(conf, target);

        if (string.IsNullOrEmpty(FilterName))
        {
            conf.SolutionFolder = "Puzzles";
        }
        else
        {
            conf.SolutionFolder = string.Format("Puzzles/{0}", FilterName);
        }

        conf.ReferencesByPath.Add(@"[conf.ProjectPath]\..\..\..\bin\CGFCodeGenerator\CGFCodeGenerator.exe");

        conf.AddPublicDependency<CGFFrameworkCSharpProject>(target);
    }

    private string _filterName = "";
    public string FilterName
    {
        get { return _filterName; }
        set { SetProperty(ref _filterName, value); }
    }
}
