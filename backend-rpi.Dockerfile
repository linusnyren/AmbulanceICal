FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-arm64v8 AS build
WORKDIR /app

# Prevent 'Warning: apt-key output should not be parsed (stdout is not a terminal)'
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=1
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
RUN dotnet dev-certs https

COPY . .
COPY ./AmbulanceICal/appsettings.json /app/appsettings.json

#Build project
RUN dotnet publish AmbulanceICal.sln -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim-arm64v8 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "/app/out/AmbulanceICal.dll"]