using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]
[module: Sharpmake.Include("cgfcore.sharpmake.cs")]
[module: Sharpmake.Include("imgui.sharpmake.cs")]

[Generate]
class CGFImGuiProject : CGFProject
{
    public CGFImGuiProject()
    {
        AddTargets(CGFTargets.GetCommonTargetsNoPreprocessToFile());

        Name = "CGFImGui";
        FolderName = "cgfImGui";
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.Lib;

        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "CGFImGui";

        conf.IncludePaths.Add("[project.SourceRootPath]");

        conf.LibraryFiles.Add("d3d11.lib");
        conf.LibraryFiles.Add("d3dcompiler.lib");
        conf.LibraryFiles.Add("dxgi.lib");

        conf.ExportDefines.Add("CODING_IMGUI");

        conf.AddPublicDependency<CGFCoreProject>(target);
        conf.AddPublicDependency<ImGuiProject>(target);
    }
}
