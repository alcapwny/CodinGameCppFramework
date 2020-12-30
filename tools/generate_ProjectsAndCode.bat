:: Generates the csharp projects for the code generator, runs the code generator, then generates the actual codingame solution

@pushd %~dp0

call _generate_ProjectsForCodeGen.bat

@echo off
if %ERRORLEVEL% NEQ 0 ( 
    pause
    popd
    exit /B %ERRORLEVEL%
)
@echo on

call generate_Code.bat

@echo off
if %ERRORLEVEL% NEQ 0 ( 
    pause
    popd
    exit /B %ERRORLEVEL%
)
@echo on

call _generate_Projects.bat

@echo off
if %ERRORLEVEL% NEQ 0 ( 
    pause
    popd
    exit /B %ERRORLEVEL%
)
@echo on

@popd