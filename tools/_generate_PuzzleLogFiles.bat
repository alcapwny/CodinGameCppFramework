:: Generates the local input file from the log file that contains the contents logged in the codingame IDE

@echo off
pushd %~dp0

if "%~1"=="" (
    set /p puzzleName="Enter puzzle name: "
) else (
    set puzzleName=%~1
)

copy NUL ..\data\%puzzleName%Log.txt
echo Created ..\data\%puzzleName%Log.txt
echo ..\bin\cgflogparser.exe %puzzleName%Log.txt %puzzleName%.txt > ..\data\%puzzleName%LogParser.bat
echo Created ..\data\%puzzleName%LogParser.bat

popd
@echo on