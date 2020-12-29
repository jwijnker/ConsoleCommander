@echo off

set filename=%1
IF "%filename%"=="" GOTO NOFILE

set /p key="Your Nuget ApiKey: "
IF "%key%"=="" GOTO NOKEY

echo %filename%
echo %key%

dotnet nuget push "%filename%" --api-key %key% --source https://api.nuget.org/v3/index.json
GOTO END

:NOFILE
echo No file to push given.
GOTO END

:NOKEY
echo No API key given.
GOTO END

:END
pause