using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]
[module: Sharpmake.Include("cgfcsharpproject.sharpmake.cs")]
[module: Sharpmake.Include("cgfimgui.sharpmake.cs")]

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

        conf.IncludePaths.Add(@"[project.SourceRootPath]\source");
        conf.IncludePaths.Add(@"[project.SourceRootPath]\generated");

        if (string.IsNullOrEmpty(FilterName))
        {
            conf.SolutionFolder = "Puzzles";
        }
        else
        {
            conf.SolutionFolder = string.Format("Puzzles/{0}", FilterName);
        }

        conf.AddPublicDependency<CGFFrameworkProject>(target);
        if (!CGFProject.IsPreprocessToFile(target.Optimization))
        {
            conf.AddPublicDependency<CGFImGuiProject>(target);
        }

        if (!CGFProject.IsPreprocessToFile(target.Optimization))
        {
            //Define to indicate that we are compiling for local debugging
            conf.Defines.Add("CODING_LOCALDEBUGGING");

            if (target.Optimization == Optimization.Debug)
            {
                conf.Options.Add(Options.Vc.Compiler.Inline.Disable); //Some functions not marked as inline were being inlined...
            }
        }
        else
        {
            //Define so that system includes are ignored by the preprocessor
            conf.AdditionalCompilerOptions.Add(@"/DCODING_EXLUDESYSTEMHEADERS=");

            conf.Options.Add(Options.Vc.Compiler.GenerateProcessorFile.WithoutLineNumbers);
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
