#! /bin/bash
cd publish || exit;nohup dotnet Server.dll --Process=9999  --Develop=0 --LogLevel=2  --AppType=Watcher &