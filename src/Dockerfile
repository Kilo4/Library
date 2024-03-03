#FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
#
## Ensure we listen on any IP Address 
#ENV DOTNET_URLS=http://+8000
#
#WORKDIR /app
#EXPOSE 8000
#EXPOSE 8001
#
## Copy the project file and restore dependencies
#COPY *.csproj ./
#RUN dotnet restore
#
#COPY . ./
#RUN dotnet publish -c Release -o out
#
## Create a runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
#WORKDIR /app
#COPY --from=build /app/out ./
#
## # Expose the port your application listens on
## EXPOSE 8000
#
## Command to run the application
#ENTRYPOINT ["dotnet", "Webapi.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Webapi.csproj", "Webapi/"]
RUN dotnet restore "src/Webapi.csproj"
COPY . .
WORKDIR "/src/Webapi"
RUN dotnet build "Webapi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Webapi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webapi.dll"]
