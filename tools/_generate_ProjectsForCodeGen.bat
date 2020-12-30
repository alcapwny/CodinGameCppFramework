:: Runs Sharpmake to generate the minimal codingame solution that only contains the csharp projects needed for the code generation

@pushd %~dp0

..\dependencies\bin\Sharpmake\Sharpmake.Application.exe "/sources(@"Sharpmake.CGF\main.codegen.sharpmake.cs") /verbose"

@popd

@exit /B %ERRORLEVEL%