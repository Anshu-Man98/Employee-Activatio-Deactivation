FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release  -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1

WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet","EmployeeDeactivation.dll"]