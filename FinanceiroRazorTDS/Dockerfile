# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /source

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2: Build the runtime image for backend
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS backend
WORKDIR /app
COPY --from=build-env /app .
HEALTHCHECK --interval=5m --timeout=3s CMD curl -f http://localhost/ || exit 1

ENTRYPOINT ["dotnet", "FinanceiroRazorTDS.dll"]  # Confirme se o nome está correto

# Stage 3: Build the runtime image for frontend
# This is just for example, in real scenarios Razor Pages are served by ASP.NET Core
FROM nginx:alpine AS frontend
COPY --from=build-env /source/wwwroot /usr/share/nginx/html
COPY --from=build-env /source/Pages /usr/share/nginx/html
