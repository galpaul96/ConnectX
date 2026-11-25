FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ConnectX.Web/ConnectX.Web.csproj", "ConnectX.Web/"]
COPY ["ConnectX.Web.ServiceDefaults/ConnectX.Web.ServiceDefaults.csproj", "ConnectX.Web.ServiceDefaults/"]
RUN dotnet restore "ConnectX.Web/ConnectX.Web.csproj"

COPY . .
WORKDIR /src/ConnectX.Web
RUN dotnet publish "ConnectX.Web.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false \
    && test -f /app/publish/wwwroot/_framework/blazor.web.js \
    && grep -Fq '"Route":"_framework/blazor.web.js"' /app/publish/ConnectX.Web.staticwebassets.endpoints.json

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN test -f /app/wwwroot/_framework/blazor.web.js
USER $APP_UID
ENTRYPOINT ["dotnet", "ConnectX.Web.dll"]
