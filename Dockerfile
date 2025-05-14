# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the solution file
COPY *.sln ./

# Copy all the project files into their corresponding folders
COPY Connect/Connect.API.csproj ./Connect/
COPY Application/Connect.Application.csproj ./Application/
COPY Domain/Connect.Core.csproj ./Domain/
COPY Infrastructure/Connect.Infrastructure.csproj ./Infrastructure/

# Restore dependencies
RUN dotnet restore Connect/Connect.API.csproj

# Install dotnet-ef tool in the build stage
RUN dotnet tool install --global dotnet-ef

# Copy the entire source code into the container
COPY . .

# Build the application
RUN dotnet build Connect/Connect.API.csproj -c Release -o /app/build

# Stage 2: Publish the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory
WORKDIR /app

# Expose port 80
EXPOSE 80

# Copy the published output from the build stage
COPY --from=build /app/build .

# Set the entrypoint
ENTRYPOINT ["dotnet", "Connect.API.dll"]
