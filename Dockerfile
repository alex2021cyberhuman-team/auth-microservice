FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG CONFIG=Debug

WORKDIR /src

COPY ["Conduit.Auth.WebApi/Conduit.Auth.WebApi.csproj", "Conduit.Auth.WebApi/"]
RUN dotnet restore "Conduit.Auth.WebApi/Conduit.Auth.WebApi.csproj"
COPY . .
WORKDIR "/src/Conduit.Auth.WebApi"
RUN dotnet build "Conduit.Auth.WebApi.csproj" -c $CONFIG -o /app/build

FROM build AS publish

ARG CONFIG=Debug

RUN dotnet publish "Conduit.Auth.WebApi.csproj" -c $CONFIG -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conduit.Auth.WebApi.dll"]
