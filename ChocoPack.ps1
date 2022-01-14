dotnet publish -o .\tools\ --self-contained True --runtime win-x64 /property:PublishTrimmed=True /property:PublishSingleFile=True
cpack