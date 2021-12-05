FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ARG ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ARG CONFIG=Debug
ENV CONFIG=$CONFIG
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ARG CONFIG=Debug
ENV CONFIG=$CONFIG
WORKDIR /src
COPY ["Conduit.Auth.WebApi/Conduit.Auth.WebApi.csproj", "Conduit.Auth.WebApi/"]
RUN dotnet restore "Conduit.Auth.WebApi/Conduit.Auth.WebApi.csproj"
COPY . .
WORKDIR "/src/Conduit.Auth.WebApi"
RUN dotnet build "Conduit.Auth.WebApi.csproj" -c $CONFIG -o /app/build

FROM build AS publish
RUN dotnet publish "Conduit.Auth.WebApi.csproj" -c $CONFIG -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conduit.Auth.WebApi.dll"]
