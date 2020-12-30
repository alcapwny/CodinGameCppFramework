:: Run the code generator to generate source files from csharp definitions

@pushd %~dp0

..\bin\CGFCodeGenerator\CGFCodeGenerator.exe ..\build\generated\codingamecodegen.sln

@popd

@exit /B %ERRORLEVEL%