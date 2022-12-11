nuget restore ./Client/Client.sln
MSBuild.exe ./Client/Client.sln

cd WebClient
dotnet restore
dotnet build 

cd CourseWork\Sea Battle
dotnet restore
dotnet build 
