:: Generates the template code for a puzzle

@echo off
pushd %~dp0

set /p puzzleName="Enter puzzle name: "
..\bin\cgfpuzzlegenerator\CGFPuzzleGenerator.exe %puzzleName% "..\codingamePuzzles" "CGFPuzzleTemplate\$puzzlename$.cpp" "CGFPuzzleTemplate\$puzzlename$InputData.cs"
_generate_PuzzleLogFiles %puzzleName%

popd
@echo on