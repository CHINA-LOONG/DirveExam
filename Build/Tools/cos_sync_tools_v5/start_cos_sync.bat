@echo off
set cur_dir=%CD%
cd %cur_dir%
set my_java_cp=.;%cur_dir%\dep\*;%cur_dir%\src\main\resources
java -Dfile.encoding=UTF-8 -cp "%my_java_cp%" com.qcloud.cos_sync_tools_xml.App
pause>nul
