# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (5000/8080 depending on your app)
EXPOSE 8080
# ASP.NET Core defaults to port 8080 in containers
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "open-family-map-api.dll"]
