using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgftargets.sharpmake.cs")]

public abstract class CGFCSharpProject : CSharpProject
{
    public CGFCSharpProject()
    {
        AddTargets(CGFTargets.GetCommonTargetsNoRetail());

        IsFileNameToLower = true;

        RootPath = @"[project.CodinGameRootPath]\[project.FolderName]";
        SourceRootPath = @"[project.CodinGameRootPath]\[project.FolderName]";
    }

    [Configure]
    public virtual void ConfigureAll(Project.Configuration conf, Target target)
    {
        //Paths
        conf.ProjectPath = @"[project.SharpmakeCsPath]\..\..\build\generated\projects";

        conf.IntermediatePath = @"[conf.ProjectPath]\..\..\..\temp\intermediate\[project.Name]_[target.Platform]_[target.Optimization]";

        if (conf.Output == Configuration.OutputType.Exe)
        {
            conf.TargetPath = @"[conf.ProjectPath]\..\..\..\bin\[project.Name]\";
        }
        else
        {
            conf.TargetPath = @"[conf.ProjectPath]\..\..\..\temp\output\[project.Name]_[target.Platform]_[target.Optimization]";
        }
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

