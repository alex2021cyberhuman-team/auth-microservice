# Require bind to current directory

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Conduit.Auth.WebApi
ARG CONFIG=Debug
ENV CONFIG=$CONFIG
ENV DOTNET_USE_POLLING_FILE_WATCHER=0
ENV DOTNET_WATCH_SUPPRESS_MSBUILD_INCREMENTALISM=1
ENTRYPOINT dotnet watch run --urls=http://+:80;https://+:443 -c=${CONFIG} -v