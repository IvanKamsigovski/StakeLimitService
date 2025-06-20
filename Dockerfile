# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .
COPY src/appsettings.json src/StakeLimit.Presentation.Api/appsettings.json

# Restore and publish from the correct path
RUN dotnet restore src/StakeLimit.Presentation.Api/StakeLimit.Presentation.Api.csproj
RUN dotnet publish src/StakeLimit.Presentation.Api/StakeLimit.Presentation.Api.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "StakeLimit.Presentation.Api.dll"]
