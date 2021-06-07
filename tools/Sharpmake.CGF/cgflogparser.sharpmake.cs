using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]

[Generate]
class CGFLogParserProject : CGFProject
{
    public CGFLogParserProject()
    {
        AddTargets(CGFTargets.GetCommonTargetsNoPreprocessToFile());

        Name = "CGFLogParser";
        FolderName = @"tools\CGFLogParser";
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "Tools";
    }
}
