#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MailBot.Webhook/NuGet.Config", "MailBot.Webhook/"]
COPY ["Webhook/NuGet.Config", "Webhook/"]
COPY ["MailBot.Webhook/MailBot.Webhook.csproj", "MailBot.Webhook/"]
COPY ["Webhook/WebhookApi.csproj", "Webhook/"]
RUN dotnet restore "MailBot.Webhook/MailBot.Webhook.csproj"
COPY . .
WORKDIR "/src/MailBot.Webhook"
RUN dotnet build "MailBot.Webhook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MailBot.Webhook.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailBot.Webhook.dll"]