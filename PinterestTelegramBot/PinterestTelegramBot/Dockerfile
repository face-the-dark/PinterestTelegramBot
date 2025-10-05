# ----------- Build stage -----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проект и восстанавливаем зависимости
COPY ["PinterestTelegramBot/PinterestTelegramBot.csproj", "PinterestTelegramBot/"]
RUN dotnet restore "PinterestTelegramBot/PinterestTelegramBot.csproj"

# Копируем остальное и собираем
COPY . .
WORKDIR "/src/PinterestTelegramBot"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ----------- Runtime stage -----------
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

# Копируем результат публикации
COPY --from=build /app/publish .

# Переменные окружения
ENV TELEGRAM_BOT_TOKEN=""
ENV CHANNEL_ID=""
ENV PINTEREST_SESSION=""

# Запуск
ENTRYPOINT ["dotnet", "PinterestTelegramBot.dll"]
