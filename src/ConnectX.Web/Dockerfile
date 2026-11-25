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
    /p:UseAppHost=false

RUN if [ ! -f /app/publish/wwwroot/_framework/blazor.web.js ]; then \
    echo "Blazor framework script missing from dotnet publish output" >&2; \
    find /app/publish -type f -iname 'blazor.web*' -print; \
    exit 1; \
fi

RUN if ! grep -Fq 'blazor.web' /app/publish/ConnectX.Web.staticwebassets.endpoints.json; then \
    echo "Blazor framework script missing from the static web assets manifest" >&2; \
    grep -oE '.{0,80}blazor\.web.{0,120}' /app/publish/ConnectX.Web.staticwebassets.endpoints.json | head -n 10 || true; \
    exit 1; \
fi

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN if [ ! -f /app/wwwroot/_framework/blazor.web.js ]; then \
    echo "Blazor framework script missing after copying publish output into the runtime image" >&2; \
    exit 1; \
fi
USER $APP_UID
ENTRYPOINT ["dotnet", "ConnectX.Web.dll"]
