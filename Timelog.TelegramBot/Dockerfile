#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Timelog.TelegramBot/Timelog.TelegramBot.csproj", "Timelog.TelegramBot/"]
COPY ["Timelog.ApiClient/Timelog.ApiClient.csproj", "Timelog.ApiClient/"]
COPY ["Timelog.Services/Timelog.Services.csproj", "Timelog.Services/"]
COPY ["Timelog.Core/Timelog.Core.csproj", "Timelog.Core/"]
RUN dotnet restore "Timelog.TelegramBot/Timelog.TelegramBot.csproj"
COPY . .
WORKDIR "/src/Timelog.TelegramBot"
RUN dotnet build "Timelog.TelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Timelog.TelegramBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Timelog.TelegramBot.dll"]