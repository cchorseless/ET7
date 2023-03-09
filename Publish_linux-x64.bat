dotnet publish -r linux-x64 --no-self-contained --no-dependencies -c Release
set OUTPUT_ROOT=.\Publish\linuxx64
del /s /q %OUTPUT_ROOT%\Bin\*
del /s /q %OUTPUT_ROOT%\Config\*
xcopy /s /y /i /e /q .\Bin\linux-x64\publish\* %OUTPUT_ROOT%\Bin\
xcopy /s /y /i /e /q .\Config\* %OUTPUT_ROOT%\Config\*
xcopy /s /y /i /e /q %OUTPUT_ROOT%\Bin\runtimes\linux\native\* %OUTPUT_ROOT%\Bin\*
del /s /q %OUTPUT_ROOT%\Bin\runtimes\*