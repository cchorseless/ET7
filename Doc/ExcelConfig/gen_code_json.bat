set WORKSPACE=..

set GEN_CLIENT=%WORKSPACE%\Tools\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=%WORKSPACE%\ExcelConfig
set UNITY_ROOT=%WORKSPACE%\ExcelConfig\Gen

%GEN_CLIENT%  -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir %UNITY_ROOT% ^
 --output_data_dir %UNITY_ROOT%\json ^
 --gen_types code_typescript_json,data_json ^
 --typescript:embed_bright_types ^
 -s client

pause