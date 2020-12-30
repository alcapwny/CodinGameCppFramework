using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]
[module: Sharpmake.Include("cgfcsharpproject.sharpmake.cs")]

[Generate]
class CGFPuzzleGeneratorCSharpProject : CGFCSharpProject
{
    public CGFPuzzleGeneratorCSharpProject()
    {
        RootPath = @"[project.CodinGameRootPath]\tools\[project.FolderName]";
        SourceRootPath = @"[project.CodinGameRootPath]\tools\[project.FolderName]";

        Name = "CGFPuzzleGenerator";
        FolderName = "CGFPuzzleGenerator";
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "Tools";

        conf.ReferencesByName.Add("System");
        conf.ReferencesByName.Add("System.Text.RegularExpressions");
    }
}
