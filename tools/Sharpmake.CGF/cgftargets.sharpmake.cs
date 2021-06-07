using Sharpmake; // contains the entire Sharpmake object library.

class CGFTargets
{
    public static Target[] GetCommonTargets()
    {
        return new Target[]{
            new Target(
                Platform.win32 | Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release | Optimization.Retail, //Retail is considered the PreprocessToFile target
                OutputType.Lib,
                Blob.NoBlob,
                BuildSystem.MSBuild,
                DotNetFramework.v4_7_2)};
    }

    public static Target[] GetCommonTargetsNoPreprocessToFile()
    {
        return new Target[]{
            new Target(
                Platform.win32 | Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release,
                OutputType.Lib,
                Blob.NoBlob,
                BuildSystem.MSBuild,
                DotNetFramework.v4_7_2)};
    }
}

