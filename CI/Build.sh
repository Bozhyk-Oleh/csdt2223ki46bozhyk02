nuget restore ./Client/Client.sln
MSBuild.exe ./Client/Client.sln
(vswhere -property installationPath)\Common7\IDE\MSTest.exe ./Client.UnitTest/bin/Debug/Client.UnitTest.dll
