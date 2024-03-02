FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+8000

WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port your application listens on
EXPOSE 8000

# Command to run the application
ENTRYPOINT ["dotnet", "Webapi.dll"]
