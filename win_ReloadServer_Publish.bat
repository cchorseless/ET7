dotnet publish -r win-x64 --no-self-contained --no-dependencies -c Release
set OUTPUT_ROOT=.\Publish\winx64

xcopy /s /y /i /e /q .\Bin\win-x64\publish\Hotfix.deps.json %OUTPUT_ROOT%\Bin\
xcopy /s /y /i /e /q .\Bin\win-x64\publish\Hotfix.dll %OUTPUT_ROOT%\Bin\
xcopy /s /y /i /e /q .\Bin\win-x64\publish\Hotfix.pdb %OUTPUT_ROOT%\Bin\
xcopy /s /y /i /e /q .\Config\* %OUTPUT_ROOT%\Config\*