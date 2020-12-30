using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]

[Generate]
class CGFFrameworkProject : CGFProject
{
    public CGFFrameworkProject()
    {
        AddTargets(CGFTargets.GetCommonTargets());

        Name = "CodinGameFramework";
        FolderName = "codingameFramework";
    }

    public override void ConfigureAll(Project.Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.Lib;

        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "Framework";
    }
}

[Generate]
class CGFFrameworkCSharpProject : CGFCSharpProject
{
    public CGFFrameworkCSharpProject()
    {
        Name = "CodinGameFrameworkCSharp";
        FolderName = "codingameFramework";
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.DotNetClassLibrary;

        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "Framework";

        conf.ReferencesByPath.Add(@"[conf.ProjectPath]\..\..\..\bin\CGFCodeGenerator\CGFCodeGenerator.exe");
    }
}
