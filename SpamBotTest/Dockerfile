FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpamBotTest/SpamBotTest.csproj", "SpamBotTest/"]
RUN dotnet restore "SpamBotTest/SpamBotTest.csproj"
COPY . .
WORKDIR "/src/SpamBotTest"
RUN dotnet build "SpamBotTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpamBotTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpamBotTest.dll"]
