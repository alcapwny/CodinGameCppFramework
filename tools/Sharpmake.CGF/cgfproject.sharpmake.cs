using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgftargets.sharpmake.cs")]

public abstract class CGFProject : Project
{
    public static readonly string PreprocessToFile = "PreprocessToFile";

    public CGFProject()
    {
        RootPath = @"[project.CodinGameRootPath]\[project.FolderName]";
        SourceRootPath = @"[project.CodinGameRootPath]\[project.FolderName]";
    }

    [Configure]
    public virtual void ConfigureAll(Project.Configuration conf, Target target)
    {
        conf.IncludePaths.Add(@"[project.SourceRootPath]\source");
        conf.IncludePaths.Add(@"[project.SourceRootPath]\generated");

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
        conf.Options.Add(Options.Vc.Compiler.MinimalRebuild.Disable);

        conf.Options.Add(Options.Vc.Linker.GenerateMapFile.Disable);
    }

    private string _codingameRootPath = @"..\..";
    public string CodinGameRootPath
    {
        get { return _codingameRootPath; }
        set { SetProperty(ref _codingameRootPath, value); }
    }

    private string _folderName = "";
    public string FolderName
    {
        get { return _folderName; }
        set { SetProperty(ref _folderName, value); }
    }
}

