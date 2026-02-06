# 1. Очистка
Remove-Item -Recurse -Force .artifacts/publish -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path .artifacts/publish -Force | Out-Null

$project = "FilesToXml.Console/FilesToXml.Console.csproj"

# 2. Публикация с правильными параметрами
Write-Host "Publishing win-x64..." -ForegroundColor Cyan
dotnet publish $project -c Release -r win-x64 --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:IncludeAllContentForSelfExtract=true `
    /p:EnableCompressionInSingleFile=true `
    -o .artifacts/publish/win-x64

Write-Host "Publishing linux-x64..." -ForegroundColor Cyan
dotnet publish $project -c Release -r linux-x64 --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:IncludeAllContentForSelfExtract=true `
    /p:EnableCompressionInSingleFile=true `
    -o .artifacts/publish/linux-x64

Write-Host "Publishing osx-x64..." -ForegroundColor Cyan
dotnet publish $project -c Release -r osx-x64 --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:IncludeAllContentForSelfExtract=true `
    /p:EnableCompressionInSingleFile=true `
    -o .artifacts/publish/osx-x64

Write-Host "Publishing osx-arm64..." -ForegroundColor Cyan
dotnet publish $project -c Release -r osx-arm64 --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:IncludeAllContentForSelfExtract=true `
    /p:EnableCompressionInSingleFile=true `
    -o .artifacts/publish/osx-arm64

# 3. Архивирование
Write-Host "Archiving..." -ForegroundColor Cyan
Compress-Archive -Path .artifacts/publish/win-x64/* -DestinationPath .artifacts/publish/FilesToXml-win-x64.zip -Force
tar -czf .artifacts/publish/FilesToXml-linux-x64.tar.gz -C .artifacts/publish/linux-x64 .
tar -czf .artifacts/publish/FilesToXml-osx-x64.tar.gz -C .artifacts/publish/osx-x64 .
tar -czf .artifacts/publish/FilesToXml-osx-arm64.tar.gz -C .artifacts/publish/osx-arm64 .

Write-Host "Done! Archives are in .artifacts/publish/" -ForegroundColor Green