#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src
COPY RedisConsumerService/RedisConsumerService.csproj RedisConsumerService/
COPY Shared.Redis/Shared.Redis.csproj Shared.Redis/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore RedisConsumerService/RedisConsumerService.csproj
COPY . .
RUN dotnet build RedisConsumerService/RedisConsumerService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish RedisConsumerService/RedisConsumerService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisConsumerService.dll"]