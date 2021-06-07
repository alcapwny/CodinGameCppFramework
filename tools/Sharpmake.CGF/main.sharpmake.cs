using Sharpmake; // contains the entire Sharpmake object library.

using System;
using System.Collections.Generic;

[module: Sharpmake.Include("cgftargets.sharpmake.cs")]

[module: Sharpmake.Include("cgflogparser.sharpmake.cs")]
[module: Sharpmake.Include("cgfpuzzlegenerator.sharpmake.cs")]

[module: Sharpmake.Include("cgfframework.sharpmake.cs")]
[module: Sharpmake.Include("cgfpuzzle.sharpmake.cs")]
[module: Sharpmake.Include("cgfpuzzleprojectgenerator.sharpmake.cs")]

[Generate]
class CodinGameSolution : Solution
{
    private List<Type> _codingamePuzzleProjectTypes;
    private List<Type> _codingamePuzzleCSharpProjectTypes;

    public CodinGameSolution()
    {
        Name = "codingame";

        AddTargets(CGFTargets.GetCommonTargets());

        CGFPuzzleProjectGenerator codingamePuzzlesProjectGenerator = new CGFPuzzleProjectGenerator();

        CGFPuzzleProjectGenerationParam[] projectGenerationParam = 
        {
            new CGFPuzzleProjectGenerationParam{ ProjectType = typeof(CGFPuzzleProject), FileSearchPattern = "*.cpp" },
            new CGFPuzzleProjectGenerationParam{ ProjectType = typeof(CGFPuzzleCSharpProject), FileSearchPattern = "*.cs" }
        };

        List<Type>[] generatedProjectTypes = new List<Type>[projectGenerationParam.Length];
        for(int i = 0; i < generatedProjectTypes.Length; ++i)
        {
            generatedProjectTypes[i] = new List<Type>();
        }

        generatedProjectTypes = codingamePuzzlesProjectGenerator.GenerateCodinGamePuzzleProjects(SharpmakeCsPath, projectGenerationParam);
        _codingamePuzzleProjectTypes = generatedProjectTypes[0];
        _codingamePuzzleCSharpProjectTypes = generatedProjectTypes[1];
    }

    [Configure]
    public void ConfigureAll(Solution.Configuration conf, Target target)
    {
        conf.SolutionPath = @"[solution.SharpmakeCsPath]\..\..\build\generated";

        if (CGFProject.IsPreprocessToFile(target.Optimization))
        {
            conf.Name = CGFProject.PreprocessToFile; 
        }

        conf.AddProject<CGFFrameworkProject>(target);

        foreach (Type projectType in _codingamePuzzleProjectTypes)
        {
            conf.AddProject(projectType, target);
        }

        if (!CGFProject.IsPreprocessToFile(target.Optimization))
        {
            conf.AddProject<CGFFrameworkCSharpProject>(target);

            foreach (Type projectType in _codingamePuzzleCSharpProjectTypes)
            {
                conf.AddProject(projectType, target);
            }
        }
    }
}

[Generate]
class CodinGameToolsSolution : Solution
{
    public CodinGameToolsSolution()
    {
        Name = "codingameTools";

        AddTargets(CGFTargets.GetCommonTargetsNoPreprocessToFile());
    }

    [Configure]
    public void ConfigureAll(Solution.Configuration conf, Target target)
    {
        conf.SolutionPath = @"[solution.SharpmakeCsPath]\..\..\build\generated";

        if (!CGFProject.IsPreprocessToFile(target.Optimization))
        {
            conf.AddProject<CGFLogParserProject>(target);
            conf.AddProject<CGFPuzzleGeneratorCSharpProject>(target);
        }
    }
}

public static class CodinGameSolutionGenerator
{
    [Sharpmake.Main]
    public static void SharpmakeMain(Sharpmake.Arguments arguments)
    {
        KitsRootPaths.SetUseKitsRootForDevEnv(DevEnv.vs2019, KitsRootEnum.KitsRoot10, Options.Vc.General.WindowsTargetPlatformVersion.v10_0_17763_0);

        arguments.Generate<CodinGameSolution>();
        arguments.Generate<CodinGameToolsSolution>();
    }
}