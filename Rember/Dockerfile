FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Rember/Rember.csproj", "Rember/"]
RUN dotnet restore "Rember/Rember.csproj"
COPY . .
WORKDIR "/src/Rember"
RUN dotnet build "Rember.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rember.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rember.dll"]
