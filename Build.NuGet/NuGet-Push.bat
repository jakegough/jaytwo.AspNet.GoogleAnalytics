call NuGet-Settings.cmd
call NuGet-Settings.private.cmd

"%NUGET_EXE%" push "%NUGET_NUPKG%" -s "%NUGET_REPOSITORY_URL%" "%NUGET_API_KEY%"

pause