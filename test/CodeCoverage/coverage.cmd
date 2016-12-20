@echo off

cd test\CodeCoverage

nuget restore packages.config -PackagesDirectory .

cd ..\GoalSetter.Tests

dotnet restore

cd ..
cd ..

rem The -threshold options prevents this taking ages...
test\CodeCoverage\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:"test test\GoalSetter.Tests -c Release -f net451" -threshold:10 -register:user -filter:"+[GoalSetter*]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:All -returntargetcode -output:.\GoalSetter.Coverage.xml

if %errorlevel% neq 0 exit /b %errorlevel%

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "GoalSetter.Coverage.xml"