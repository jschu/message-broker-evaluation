#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY PublisherService/PublisherService.csproj PublisherService/
COPY Shared.RabbitMQ/Shared.RabbitMQ.csproj Shared.RabbitMQ/
COPY Shared.Kafka/Shared.Kafka.csproj Shared.Kafka/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore PublisherService/PublisherService.csproj
COPY . .
RUN dotnet build PublisherService/PublisherService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish PublisherService/PublisherService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
EXPOSE 80
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublisherService.dll"]