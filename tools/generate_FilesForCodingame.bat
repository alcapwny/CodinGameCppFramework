:: Generate source files to be used in the CodinGame IDE

@echo off

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

if %ERRORLEVEL% NEQ 0 ( 
    echo Error: Exit code %ERRORLEVEL% calling %vsmsbuildcmdPath%.
    pause
    exit /B %ERRORLEVEL%
)

@echo on

:: Build codingame puzzles
msbuild %~dp0..\build\generated\codingame.sln "/property:Configuration=PreprocessToFile;Platform=Win32"