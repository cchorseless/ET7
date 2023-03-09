#! /bin/bash
cd Bin || exit;nohup dotnet App.dll --Process=9999 --StartConfig=StartConfig/Localhost --Develop=0 --LogLevel=2 --AppType=Watcher &