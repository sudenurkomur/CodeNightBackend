FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CodeNight.sln ./
COPY src/Domain/CodeNight.Domain.csproj src/Domain/
COPY src/Application/CodeNight.Application.csproj src/Application/
COPY src/Infrastructure/CodeNight.Infrastructure.csproj src/Infrastructure/
COPY src/WebApi/CodeNight.WebApi.csproj src/WebApi/

RUN dotnet restore

COPY src/ src/

RUN dotnet publish src/WebApi/CodeNight.WebApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CodeNight.WebApi.dll"]
