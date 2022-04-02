FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ARG ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ARG CONFIG=Debug
ENV CONFIG=$CONFIG
WORKDIR /app
EXPOSE 80

FROM base AS final
WORKDIR /app
COPY ./app/publish .
ENTRYPOINT ["dotnet", "Conduit.Auth.WebApi.dll"]
HEALTHCHECK --timeout=120s --retries=120 CMD curl --fail http://localhost/health || exit