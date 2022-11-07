nuget restore ./Client/Client.sln
msbuild.exe ./Client/Client.sln
dotnet test ./Client/Client.sln
