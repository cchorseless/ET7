set WORKSPACE=..\..

set GEN_CLIENT=..\Tools\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=..\ExcelConfig
set SERVER_ROOT=%WORKSPACE%\DotNet
set CONFIG_ROOT=%WORKSPACE%\Config

%GEN_CLIENT%  -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_data_dir %CONFIG_ROOT%\Luban ^
 --gen_types data_bin ^

 -s all

pause