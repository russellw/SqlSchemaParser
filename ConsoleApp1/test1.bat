cd \sqlschemaparser\consoleapp1||exit /b
msbuild /p:Configuration=Debug /p:Platform="Any CPU"||exit /b
echo off
"C:\SqlSchemaParser\ConsoleApp1\bin\Any CPU\Debug\net7.0\ConsoleApp1.exe" "\SqlSchemaParser\TestProject1\sql-server\cities.sql"
