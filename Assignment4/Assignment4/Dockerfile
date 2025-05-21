# Use ASP.NET Core runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["Assignment4/EventManagement.csproj", "Assignment4/"]
COPY ["DbOperation/DbOperation.csproj", "DbOperation/"]
RUN dotnet restore "Assignment4/EventManagement.csproj"

# Copy everything else
COPY . .
WORKDIR "/src/Assignment4"
RUN dotnet build "EventManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventManagement.dll"]
