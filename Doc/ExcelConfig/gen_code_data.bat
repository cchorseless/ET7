set WORKSPACE=..\..

set GEN_CLIENT=..\Tools\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=..\ExcelConfig
set SERVER_ROOT=%WORKSPACE%\DotNet
set CONFIG_ROOT=%WORKSPACE%\Config

%GEN_CLIENT%  -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir %SERVER_ROOT%\Hotfix\Config\Luban ^
 --output_data_dir %CONFIG_ROOT%\Luban ^
 --output:data:compact_json ^
 --gen_types code_cs_bin,data_bin,data_json ^

 -s server

pause