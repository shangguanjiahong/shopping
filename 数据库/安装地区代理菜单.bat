@echo off
chcp 65001 >nul
echo ====================================
echo CoreShop 地区代理管理菜单配置工具
echo ====================================
echo.
echo 请选择您的数据库类型：
echo 1. MySQL
echo 2. SQL Server
echo 3. 退出
echo.
set /p choice=请输入选择 (1-3): 

if "%choice%"=="1" goto mysql
if "%choice%"=="2" goto sqlserver
if "%choice%"=="3" goto exit
goto invalid

:mysql
echo.
echo 您选择了 MySQL 数据库
echo SQL脚本位置: MySql\地区代理菜单配置.sql
echo.
echo 请按照以下步骤操作：
echo 1. 连接到您的 CoreShop MySQL 数据库
echo 2. 执行以下命令：
echo    USE your_coreshop_database;
echo    source MySql/地区代理菜单配置.sql;
echo 3. 重启 CoreShop 应用程序
echo 4. 在角色管理中分配相应权限
echo.
echo 详细说明请参考：地区代理菜单配置说明.md
goto end

:sqlserver
echo.
echo 您选择了 SQL Server 数据库
echo SQL脚本位置: SqlServer\地区代理菜单配置.sql
echo.
echo 请按照以下步骤操作：
echo 1. 连接到您的 CoreShop SQL Server 数据库
echo 2. 打开 SqlServer\地区代理菜单配置.sql 文件
echo 3. 复制所有内容并在数据库中执行
echo 4. 重启 CoreShop 应用程序
echo 5. 在角色管理中分配相应权限
echo.
echo 详细说明请参考：地区代理菜单配置说明.md
goto end

:invalid
echo.
echo 无效的选择，请重新运行脚本
goto end

:exit
echo.
echo 退出安装程序
goto end

:end
echo.
echo 按任意键退出...
pause >nul