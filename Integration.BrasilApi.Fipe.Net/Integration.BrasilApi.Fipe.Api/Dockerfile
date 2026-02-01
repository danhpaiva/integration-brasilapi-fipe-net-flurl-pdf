# Estágio Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copia todos os .csproj primeiro para aproveitar o cache das camadas do Docker
COPY ["Integration.BrasilApi.Fipe.Api/Integration.BrasilApi.Fipe.Api.csproj", "Integration.BrasilApi.Fipe.Api/"]
COPY ["Integration.BrasilApi.Fipe.Domain/Integration.BrasilApi.Fipe.Domain.csproj", "Integration.BrasilApi.Fipe.Domain/"]
COPY ["Integration.BrasilApi.Fipe.Infrastructure/Integration.BrasilApi.Fipe.Infrastructure.csproj", "Integration.BrasilApi.Fipe.Infrastructure/"]

# 2. Restaura as dependências (agora ele enxerga todos os projetos)
RUN dotnet restore "Integration.BrasilApi.Fipe.Api/Integration.BrasilApi.Fipe.Api.csproj"

# 3. Copia o restante dos arquivos e compila
COPY . .
WORKDIR "/src/Integration.BrasilApi.Fipe.Api"
RUN dotnet build "Integration.BrasilApi.Fipe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Integration.BrasilApi.Fipe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Integration.BrasilApi.Fipe.Api.dll"]