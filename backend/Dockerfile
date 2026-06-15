# 1. Fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos todos los archivos del repositorio
COPY . .

# Compilamos el proyecto específico (ajustado a la ruta que me mostraste)
RUN dotnet publish "ArbNet/ArbNet.csproj" -c Release -o out

# 2. Fase de ejecución (imagen más pequeña)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Ejecutamos la aplicación
ENTRYPOINT ["dotnet", "ArbNet.dll"]
