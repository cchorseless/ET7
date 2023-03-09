dotnet publish -r win-x64 --no-self-contained --no-dependencies -c Release
set OUTPUT_ROOT=.\Publish\winx64
del /s /q %OUTPUT_ROOT%\Bin\*
del /s /q %OUTPUT_ROOT%\Config\*
xcopy /s /y /i /e /q .\Bin\win-x64\publish\* %OUTPUT_ROOT%\Bin\
xcopy /s /y /i /e /q .\Config\* %OUTPUT_ROOT%\Config\*
xcopy /s /y /i /e /q %OUTPUT_ROOT%\Bin\runtimes\win\native\* %OUTPUT_ROOT%\Bin\*
del /s /q %OUTPUT_ROOT%\Bin\runtimes\*