# Fase 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish "backend/ArbNet/ArbNet.csproj" -c Release -o /app/out

# Fase 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Copiamos el resultado de la fase anterior
COPY --from=build /app/out .

# Definimos el comando de inicio explícitamente
# Asegúrate de que el nombre "ArbNet.dll" sea el correcto
ENTRYPOINT ["dotnet", "ArbNet.dll"]