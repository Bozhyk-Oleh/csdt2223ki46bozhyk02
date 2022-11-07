nuget restore ./Client/Client.sln
MSBuild.exe ./Client/Client.sln
MSTest.exe ./Client.UnitTest/bin/Debug/Client.UnitTest.dll
