dotnet tool uninstall rember --global
dotnet pack
dotnet tool update --global --add-source .\Rember\nupkg\ rember  --version 0.0.4 --no-cache
