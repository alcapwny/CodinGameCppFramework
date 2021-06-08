using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]

[Generate]
class CGFCoreProject : CGFProject
{
    public CGFCoreProject()
    {
        AddTargets(CGFTargets.GetCommonTargetsNoPreprocessToFile());

        Name = "CGFCore";
        FolderName = "cgfCore";
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.Lib;

        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "CGFCore";

        conf.IncludePaths.Add("[project.SourceRootPath]");
    }
}
