dotnet build FilesToXml.Core -c Release
dotnet pack b2xtranslator/Common -c Release -o .artifacts/nuget
dotnet pack b2xtranslator/Doc -c Release -o .artifacts/nuget
dotnet pack b2xtranslator/Xls -c Release -o .artifacts/nuget
dotnet pack FilesToXml.Core -c Release -o .artifacts/nuget
dotnet restore