# Use Linux-based images instead of Windows-based images
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app


EXPOSE 5000

# Use SDK for build and runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Correct path to copy .csproj based on the Dockerfile location
COPY backen-dotnet/backen-dotnet.csproj ./  

# Restore dependencies
RUN dotnet restore "backen-dotnet.csproj"

WORKDIR /src

# Copy the entire context into the container
COPY . .  

WORKDIR "/src/backen-dotnet"

RUN dotnet tool install --global dotnet-ef
ENV PATH=$PATH:/root/.dotnet/tools

# Build the application
RUN dotnet build "backen-dotnet.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Publish the application
RUN dotnet publish "backen-dotnet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY *.csproj .
# Install required GPG tools to add Microsoft's GPG key
RUN apt-get update && apt-get install -y \
    wget \
    apt-transport-https \
    gnupg \
    && wget https://packages.microsoft.com/config/ubuntu/20.04/prod.list -O /etc/apt/sources.list.d/microsoft-prod.list \
    && wget -q https://packages.microsoft.com/keys/microsoft.asc -O- | apt-key add - \
    && apt-get update \
    && apt-get install -y dotnet-sdk-8.0

# Copy the published files from the build stage
COPY --from=publish /app/publish .

# Copy entrypoint script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Set entrypoint
ENTRYPOINT ["/app/entrypoint.sh"]
