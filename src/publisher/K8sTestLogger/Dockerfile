FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY K8sTestLogger.csproj .
COPY nuget.config .
RUN dotnet restore K8sTestLogger.csproj
COPY . .
WORKDIR /src
RUN dotnet build K8sTestLogger.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish K8sTestLogger.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "K8sTestLogger.dll"]
