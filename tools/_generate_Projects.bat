:: Runs Sharpmake to generate the codingame solution

@pushd %~dp0

..\dependencies\bin\Sharpmake\Sharpmake.Application.exe "/sources(@"Sharpmake.CGF\main.sharpmake.cs") /verbose"

@popd

@exit /B %ERRORLEVEL%
