@echo off

reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath > nul 2>&1
if ERRORLEVEL 1 goto MissingMSBuildRegistry

for /f "skip=2 tokens=2,*" %%A in ('reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath') do SET MSBUILDDIR=%%B

IF NOT EXIST %MSBUILDDIR%nul goto MissingMSBuildToolsPath
IF NOT EXIST %MSBUILDDIR%msbuild.exe goto MissingMSBuildExe

::Unit Test
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq.Ninject.Tests\EntityFramework.Testing.Moq.Ninject.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq.Ninject.Tests\EntityFramework.Testing.Moq.Ninject.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute.Ninject.Tests\EntityFramework.Testing.NSubstitute.Ninject.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute.Ninject.Tests\EntityFramework.Testing.NSubstitute.Ninject.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy.Ninject.Tests\EntityFramework.Testing.FakeItEasy.Ninject.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy.Ninject.Tests\EntityFramework.Testing.FakeItEasy.Ninject.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq.Tests\EntityFramework.Testing.Moq.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq.Tests\EntityFramework.Testing.Moq.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute.Tests\EntityFramework.Testing.NSubstitute.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute.Tests\EntityFramework.Testing.NSubstitute.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy.Tests\EntityFramework.Testing.FakeItEasy.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy.Tests\EntityFramework.Testing.FakeItEasy.Tests.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\

"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.Moq.Ninject.Tests\bin\Release\net40\EntityFramework.Testing.Moq.Ninject.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.Moq.Ninject.Tests\bin\Release\net45\EntityFramework.Testing.Moq.Ninject.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.NSubstitute.Ninject.Tests\bin\Release\net40\EntityFramework.Testing.NSubstitute.Ninject.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.NSubstitute.Ninject.Tests\bin\Release\net45\EntityFramework.Testing.NSubstitute.Ninject.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.FakeItEasy.Ninject.Tests\bin\Release\net40\EntityFramework.Testing.FakeItEasy.Ninject.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.FakeItEasy.Ninject.Tests\bin\Release\net45\EntityFramework.Testing.FakeItEasy.Ninject.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.Moq.Tests\bin\Release\net40\EntityFramework.Testing.Moq.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.Moq.Tests\bin\Release\net45\EntityFramework.Testing.Moq.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.NSubstitute.Tests\bin\Release\net40\EntityFramework.Testing.NSubstitute.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.NSubstitute.Tests\bin\Release\net45\EntityFramework.Testing.NSubstitute.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.FakeItEasy.Tests\bin\Release\net40\EntityFramework.Testing.FakeItEasy.Tests.dll"
"tools\xunit\xunit.console.clr4.exe" "src\EntityFramework.Testing.FakeItEasy.Tests\bin\Release\net45\EntityFramework.Testing.FakeItEasy.Tests.dll"

::BUILD
"tools\nuget\nuget.exe" restore EntityFramework.Testing.sln 

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq.Ninject\EntityFramework.Testing.Moq.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.Moq.Ninject.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq.Ninject\EntityFramework.Testing.Moq.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.Moq.Ninject.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute.Ninject\EntityFramework.Testing.NSubstitute.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.NSubstitute.Ninject.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute.Ninject\EntityFramework.Testing.NSubstitute.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.NSubstitute.Ninject.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy.Ninject\EntityFramework.Testing.FakeItEasy.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.FakeItEasy.Ninject.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy.Ninject\EntityFramework.Testing.FakeItEasy.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.FakeItEasy.Ninject.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Ninject\EntityFramework.Testing.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.Ninject.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Ninject\EntityFramework.Testing.Ninject.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.Ninject.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq\EntityFramework.Testing.Moq.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.Moq.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.Moq\EntityFramework.Testing.Moq.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.Moq.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute\EntityFramework.Testing.NSubstitute.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.NSubstitute.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.NSubstitute\EntityFramework.Testing.NSubstitute.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.NSubstitute.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy\EntityFramework.Testing.FakeItEasy.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.FakeItEasy.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing.FakeItEasy\EntityFramework.Testing.FakeItEasy.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.FakeItEasy.xml

"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing\EntityFramework.Testing.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;DefineConstants="TRACE;NET40";OutPutPath=bin\Release\net40\;DocumentationFile=bin\Release\net40\EntityFramework.Testing.xml
"%MSBUILDDIR%msbuild.exe" "src\EntityFramework.Testing\EntityFramework.Testing.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\EntityFramework.Testing.xml

mkdir build
del "build\*.nupkg"

::PACK
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing\EntityFramework.Testing.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.Moq\EntityFramework.Testing.Moq.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.NSubstitute\EntityFramework.Testing.NSubstitute.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.FakeItEasy\EntityFramework.Testing.FakeItEasy.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.Ninject\EntityFramework.Testing.Ninject.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.Moq.Ninject\EntityFramework.Testing.Moq.Ninject.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.NSubstitute.Ninject\EntityFramework.Testing.NSubstitute.Ninject.nuspec" -OutputDirectory build
"tools\nuget\nuget.exe" pack "src\EntityFramework.Testing.FakeItEasy.Ninject\EntityFramework.Testing.FakeItEasy.Ninject.nuspec" -OutputDirectory build

::DEPLOY
"tools\nuget\nuget.exe" push "build\*.nupkg"

goto:eof
::ERRORS
::---------------------
:MissingMSBuildRegistry
echo Cannot obtain path to MSBuild tools from registry
goto:eof
:MissingMSBuildToolsPath
echo The MSBuild tools path from the registry '%MSBUILDDIR%' does not exist
goto:eof
:MissingMSBuildExe
echo The MSBuild executable could not be found at '%MSBUILDDIR%'
goto:eof