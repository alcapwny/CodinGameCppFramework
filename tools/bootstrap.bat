:: Prepares things needed to work with this framework. Builds tools and prepares the initial codingame solution.

@echo off
@pushd %~dp0

:: Get vswhere location
set vswherePath="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist %vswherePath% (
    echo Error: vswhere.exe not found in %vswherePath%.
    pause
    exit /B 1
)

for /f "usebackq tokens=*" %%i in (`%vswherePath% -latest -products * -version [16.0 -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath`) do (
  set installDir=%%i
)

set vsmsbuildcmdPath="%installDir%\Common7\Tools\VsMSBuildCmd.bat"
if not exist %vsmsbuildcmdPath% (
    echo Error: VisualStudio 16.0 or greater not found.
    pause
    exit /B 2
)

:: Call msbuild batch
echo Calling %vsmsbuildcmdPath%
call %vsmsbuildcmdPath%

@echo on

:: Build CodinGameFrameworkCodeGenerator
msbuild -t:build -restore ..\tools\CGFCodeGenerator\CGFCodeGenerator.sln "/property:Configuration=Release;Platform=Any CPU"

@echo off

if %ERRORLEVEL% NEQ 0 ( 
    pause
    exit /B %ERRORLEVEL%
)

:: Generate codingame projects/solution
echo Calling generate_ProjectsAndCode.bat
call generate_ProjectsAndCode.bat

@echo off

if %ERRORLEVEL% NEQ 0 ( 
    pause
    exit /B %ERRORLEVEL%
)

@echo on

:: Build tools
msbuild ..\build\generated\codingametools.sln "/property:Configuration=Release;Platform=Win32"

@echo off

if %ERRORLEVEL% NEQ 0 ( 
    pause
    exit /B %ERRORLEVEL%
)

popd
@echo on