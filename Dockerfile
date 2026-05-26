# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TaskManagerApi/TaskManagerApi.csproj TaskManagerApi/
RUN dotnet restore TaskManagerApi/TaskManagerApi.csproj

COPY TaskManagerApi/ TaskManagerApi/
WORKDIR /src/TaskManagerApi
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TaskManagerApi.dll"]
