#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY KafkaConsumerService/KafkaConsumerService.csproj KafkaConsumerService/
COPY Shared.Kafka/Shared.Kafka.csproj Shared.Kafka/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore KafkaConsumerService/KafkaConsumerService.csproj
COPY . .
RUN dotnet build KafkaConsumerService/KafkaConsumerService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish KafkaConsumerService/KafkaConsumerService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KafkaConsumerService.dll"]