# Этап 1: Сборка и публикация приложения
FROM ://microsoft.com AS build-env
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем оставшиеся исходные файлы и собираем релизную версию
COPY . ./
RUN dotnet publish -c Release -o out

# Этап 2: Запуск готового приложения
FROM ://microsoft.com
WORKDIR /app

# Создаем папку для базы данных SQLite, чтобы подключить к ней Volume
RUN mkdir -p /app/TodoDb

# Копируем собранные файлы из предыдущего этапа
COPY --from=build-env /app/out .

# Открываем порт, который используется ASP.NET Core по умолчанию
EXPOSE 8080

# Переменные окружения для работы приложения
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TodoApi.dll"]
