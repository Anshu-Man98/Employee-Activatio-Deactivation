
# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:2.1
WORKDIR /app
COPY --from=build-env /app/out .
COPY ["*.pdf","./"]
ENTRYPOINT ["dotnet", "EmployeeDeactivation.dll"]