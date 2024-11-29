# Etapa base para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de build para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos os arquivos do projeto
COPY . .

# Restaurar pacotes e compilar
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa final para rodar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Entrada para iniciar o servidor
ENTRYPOINT ["dotnet", "Controle_de_estoque.dll"]
