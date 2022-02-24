#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY RabbitMQConsumerService.csproj .
RUN dotnet restore RabbitMQConsumerService.csproj
COPY . .
RUN dotnet build RabbitMQConsumerService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish RabbitMQConsumerService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQConsumerService.dll"]