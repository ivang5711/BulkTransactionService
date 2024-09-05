FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# restore
COPY ["src/BulkTransactionServiceWebApi/BulkTransactionServiceWebApi.csproj", "BulkTransactionServiceWebApi/"]
RUN dotnet restore 'BulkTransactionServiceWebApi/BulkTransactionServiceWebApi.csproj'

# build
COPY ["src/BulkTransactionServiceWebApi", "BulkTransactionServiceWebApi/"]
WORKDIR /src/BulkTransactionServiceWebApi
RUN dotnet build 'BulkTransactionServiceWebApi.csproj' -c Release -o /app/build

# publish
FROM build as publish
RUN dotnet publish 'BulkTransactionServiceWebApi.csproj' -c Release -o /app/publish

# run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5067
EXPOSE 5067
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulkTransactionServiceWebApi.dll"]