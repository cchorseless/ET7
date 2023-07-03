set OUTPUT_ROOT=.\Publish\winx64

copy .\Bin\Hotfix.dll %OUTPUT_ROOT%\Bin\
copy .\Bin\Hotfix.pdb %OUTPUT_ROOT%\Bin\
xcopy /s /y /i /e /q .\Config\* %OUTPUT_ROOT%\Config\*


echo %date:~0,4%-%date:~5,2%-%date:~8,2% %time% CMD=Reload >> %OUTPUT_ROOT%\Watcher\WatcherLog.txt