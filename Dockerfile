# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código
COPY . ./
RUN dotnet publish -c Release -o /app

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Puerto de escucha
EXPOSE 8080

# Render usa PORT como variable de entorno
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "perla-metro-user.dll"]
