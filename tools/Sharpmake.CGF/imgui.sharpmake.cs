using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgfproject.sharpmake.cs")]

[Generate]
class ImGuiProject : CGFProject
{
    public ImGuiProject()
    {
        AddTargets(CGFTargets.GetCommonTargetsNoPreprocessToFile());

        Name = "ImGui";
        FolderName = @"dependencies\imgui";

        SourceFilesExcludeRegex.Add(@"\\misc\\");
        SourceFilesExcludeRegex.Add(@"\\examples\\");
        SourceFilesExcludeRegex.Add(@"\\backends\\(?!.+(_win32|_dx11).(h|cpp))"); // Remove everything from \backends\ except the win32/dx11 files


        NatvisFiles.Add(@"[project.SourceRootPath]\misc\natvis\imgui.natvis");
    }

    public override void ConfigureAll(Configuration conf, Target target)
    {
        conf.Output = Configuration.OutputType.Lib;

        base.ConfigureAll(conf, target);

        conf.SolutionFolder = "ImGui";

        conf.IncludePaths.Add("[project.SourceRootPath]");
    }
}
