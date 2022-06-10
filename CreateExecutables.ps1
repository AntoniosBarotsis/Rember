dotnet publish -o publish

dotnet publish -o tools --self-contained True /property:PublishTrimmed=True /property:PublishSingleFile=True