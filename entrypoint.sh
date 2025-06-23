#!/bin/bash
# entrypoint.sh

echo "Waiting for MySQL server to become available..."
echo "Install dotnet tools ..."

# Install dotnet-ef tool globally
dotnet tool install --global dotnet-ef

# Ensure dotnet tools are available by adding them to the PATH
export PATH=$PATH:/root/.dotnet/tools


# Sleep for 10 seconds to ensure MySQL is ready
sleep 10


# Start the application
echo "Starting the application..."
dotnet /app/backen-dotnet.dll