using System;
using System.Collections.Generic;
using Sharpmake; // contains the entire Sharpmake object library.

[module: Sharpmake.Include("cgftargets.sharpmake.cs")]

[module: Sharpmake.Include("cgfframework.sharpmake.cs")]
[module: Sharpmake.Include("cgfpuzzle.sharpmake.cs")]
[module: Sharpmake.Include("cgfpuzzleprojectgenerator.sharpmake.cs")]

[Generate]
class CodinGameCodeGenSolution : Solution
{
    private List<Type> _codingamePuzzleCSharpProjectTypes;

    public CodinGameCodeGenSolution()
    {
        Name = "codingameCodeGen";

        AddTargets(CGFTargets.GetCommonTargetsNoPreprocessToFile());

        CGFPuzzleProjectGenerator codingamePuzzlesProjectGenerator = new CGFPuzzleProjectGenerator();

        CGFPuzzleProjectGenerationParam[] projectGenerationParam =
        {
            new CGFPuzzleProjectGenerationParam{ ProjectType = typeof(CGFPuzzleCSharpProject), FileSearchPattern = "*.cs" }
        };

        List<Type>[] generatedProjectTypes = new List<Type>[projectGenerationParam.Length];
        for (int i = 0; i < generatedProjectTypes.Length; ++i)
        {
            generatedProjectTypes[i] = new List<Type>();
        }

        generatedProjectTypes = codingamePuzzlesProjectGenerator.GenerateCodinGamePuzzleProjects(SharpmakeCsPath, projectGenerationParam);
        _codingamePuzzleCSharpProjectTypes = generatedProjectTypes[0];
    }

    [Configure]
    public void ConfigureAll(Solution.Configuration conf, Target target)
    {
        conf.SolutionPath = @"[solution.SharpmakeCsPath]\..\..\build\generated";

        conf.AddProject<CGFFrameworkCSharpProject>(target);

        foreach (Type projectType in _codingamePuzzleCSharpProjectTypes)
        {
            conf.AddProject(projectType, target);
        }
    }
}

public static class CodinGameCodeGenSolutionGenerator
{
    [Sharpmake.Main]
    public static void SharpmakeMain(Sharpmake.Arguments arguments)
    {
        KitsRootPaths.SetUseKitsRootForDevEnv(DevEnv.vs2019, KitsRootEnum.KitsRoot10, Options.Vc.General.WindowsTargetPlatformVersion.v10_0_17763_0);

        arguments.Generate<CodinGameCodeGenSolution>();
    }
}