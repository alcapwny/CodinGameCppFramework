:: Delete all generated source files

@pushd %~dp0

PowerShell Remove-Item "..\codingameFramework\generated\*" -recurse
PowerShell Remove-Item "..\codingamePuzzles\*\generated\*" -recurse

@popd