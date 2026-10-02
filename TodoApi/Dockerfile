# Этап 1: Сборка и публикация приложения
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем исходники и собираем приложение
COPY . ./
RUN dotnet publish -c Release -o out

# Этап 2: Запуск готового приложения
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Создаем системную папку /data в корне контейнера, которую требует бэкенд
RUN mkdir -p /data

# Копируем собранные файлы из предыдущего этапа
COPY --from=build-env /app/out .

# Открываем порт, который используется ASP.NET Core по умолчанию
EXPOSE 8080

# Переменные окружения для работы приложения
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TodoApi.dll"]