FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ConnectX.Web/ConnectX.Web.csproj", "ConnectX.Web/"]
COPY ["ConnectX.Web.ServiceDefaults/ConnectX.Web.ServiceDefaults.csproj", "ConnectX.Web.ServiceDefaults/"]
COPY ["ConnectX.Services/ConnectX.Services.csproj", "ConnectX.Services/"]
COPY ["ConnectX.Domain/ConnectX.Domain.csproj", "ConnectX.Domain/"]
COPY ["ConnectX.Ef/ConnectX.Ef.csproj", "ConnectX.Ef/"]
COPY ["ConnectX.MongoDb/ConnectX.MongoDb.csproj", "ConnectX.MongoDb/"]
RUN dotnet restore "ConnectX.Web/ConnectX.Web.csproj"

COPY . .
WORKDIR /src/ConnectX.Web
RUN dotnet publish "ConnectX.Web.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "ConnectX.Web.dll"]
