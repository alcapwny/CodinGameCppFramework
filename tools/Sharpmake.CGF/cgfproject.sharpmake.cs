using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgftargets.sharpmake.cs")]

public abstract class CGFProject : Project
{
    public static readonly string PreprocessToFile = "PreprocessToFile";

    public static bool IsPreprocessToFile(Optimization optimization)
    {
        return optimization == Optimization.Retail;
    }

    public CGFProject()
    {
        RootPath = @"[project.CodinGameRootPath]\[project.FolderName]";
        SourceRootPath = @"[project.CodinGameRootPath]\[project.FolderName]";
    }

    [Configure]
    public virtual void ConfigureAll(Project.Configuration conf, Target target)
    {
        //Paths
        conf.ProjectPath = @"[project.SharpmakeCsPath]\..\..\build\generated\projects\";

        conf.IntermediatePath = @"[conf.ProjectPath]\..\..\..\temp\intermediate\[project.Name]_[target.Platform]_[target.Optimization]";

        if (conf.Output == Configuration.OutputType.Lib)
        {
            conf.TargetPath = @"[conf.ProjectPath]\..\..\..\temp\output\[project.Name]_[target.Platform]_[target.Optimization]";
        }
        else
        {
            conf.TargetPath = @"[conf.ProjectPath]\..\..\..\bin\";
            if (conf.VcxprojUserFile == null)
            {
                conf.VcxprojUserFile = new Configuration.VcxprojUserFileSettings();
                conf.VcxprojUserFile.LocalDebuggerWorkingDirectory = @"[conf.ProjectPath]\..\..\..\data\";
            }
        }

        //Defines
        conf.Defines.Add("_HAS_EXCEPTIONS=0");

        //Additional options
        conf.Options.Add(Options.Vc.General.CharacterSet.Unicode);

        conf.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);
        conf.Options.Add(Options.Vc.Compiler.ConformanceMode.Enable);

        conf.Options.Add(Options.Vc.Linker.GenerateMapFile.Disable);

        if (target.Optimization == Optimization.Debug)
        {
            ConfigureEditAndContinue(conf, target);
        }

        conf.Options.Add(new Sharpmake.Options.Vc.Compiler.DisableSpecificWarnings("4100")); //Warning	C4100 - unreferenced formal parameter
    }

    public void ConfigureEditAndContinue(Project.Configuration conf, Target target)
    {
        //Enable Edit and Continue
        conf.Options.Add(Options.Vc.General.DebugInformation.ProgramDatabaseEnC);
        conf.Options.Add(Options.Vc.Compiler.FunctionLevelLinking.Enable);
        conf.Options.Add(Options.Vc.Linker.Incremental.Enable);
    }

    private string _codingameRootPath = @"..\..";
    public string CodinGameRootPath
    {
        get { return _codingameRootPath; }
        set { SetProperty(ref _codingameRootPath, value); }
    }
    public string FolderName { get; set; }
}