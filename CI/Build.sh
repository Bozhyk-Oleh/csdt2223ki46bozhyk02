nuget restore ./Client/Client.sln
MSBuild.exe ./Client/Client.sln
vstest.console.exe ./Client.UnitTest/bin/Debug/Client.UnitTest.dll
