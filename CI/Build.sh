nuget restore ./Client/Client.sln
MSBuild.exe ./Client/Client.sln

cd WebClient
dotnet restore
dotnet build 
