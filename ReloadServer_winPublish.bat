rd "./Bin/win-x64/Config"
XCOPY "./Config" "./Bin/win-x64/Config" /Y /E /I
cd ./Bin
copy  "Server.Hotfix.deps.json" "./win-x64/publish"
copy  "Server.Hotfix.dll" "./win-x64/publish"
copy  "Server.Hotfix.pdb" "./win-x64/publish"
pause