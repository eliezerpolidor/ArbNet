# Usamos una imagen oficial de .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos todo el proyecto
COPY . .

# Publicamos el proyecto específico
RUN dotnet publish backend/ArbNet/ArbNet.csproj -c Release -o out

# Usamos una imagen más ligera para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Ejecutamos la aplicación
ENTRYPOINT ["dotnet", "ArbNet.dll"]