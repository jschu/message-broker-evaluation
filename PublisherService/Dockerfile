#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src
COPY PublisherService/PublisherService.csproj PublisherService/
COPY Shared.RabbitMQ/Shared.RabbitMQ.csproj Shared.RabbitMQ/
COPY Shared.Kafka/Shared.Kafka.csproj Shared.Kafka/
COPY Shared.Redis/Shared.Redis.csproj Shared.Redis/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore PublisherService/PublisherService.csproj
COPY . .
RUN dotnet build PublisherService/PublisherService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish PublisherService/PublisherService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
EXPOSE 8080

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublisherService.dll"]