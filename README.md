# 🚀 TodoList

**REST API для управления задачами**, разработанный в рамках командного проекта по продуктовому программированию.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)

![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![Vite](https://img.shields.io/badge/Vite-646CFF?style=for-the-badge&logo=vite&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![ESLint](https://img.shields.io/badge/ESLint-4B32C3?style=for-the-badge&logo=eslint&logoColor=white)

![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-In_Development-yellow?style=for-the-badge)

---

## 🎯 О проекте

**TodoApi** — серверная часть приложения для управления задачами. Предоставляет REST API для работы с пользователями и их задачами, поддерживает хранение данных в SQLite и автоматическую генерацию интерактивной документации через Swagger.
Проект разрабатывается командой в рамках учебного курса по продуктовому программированию и включает как серверную, так и клиентскую части.

---

## 🛠️ Технологический стек

### 🔧 Backend

| Технология | Версия | Назначение |
|---|---|---|
| ![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white) | 10.0 | Платформа разработки |
| ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?logo=dotnet&logoColor=white) | 10.0 | Web API |
| ![EF Core](https://img.shields.io/badge/EF_Core-10.0-512BD4?logo=dotnet&logoColor=white) | 10.0 | ORM |
| ![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white) | — | Встроенная реляционная база данных |
| ![Swagger](https://img.shields.io/badge/Swashbuckle-7.2.0-85EA2D?logo=swagger&logoColor=black) | 7.2.0 | Генерация Swagger-документации |

### 🎨 Frontend

| Технология | Назначение |
|---|---|
| ![React](https://img.shields.io/badge/React-20232A?logo=react&logoColor=61DAFB) | Библиотека для построения пользовательских интерфейсов |
| ![Vite](https://img.shields.io/badge/Vite-646CFF?logo=vite&logoColor=white) | Сборщик и dev-сервер |
| ![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=black) | Язык разработки клиентской части |
| ![React Router](https://img.shields.io/badge/React_Router-CA4245?logo=reactrouter&logoColor=white) | Маршрутизация в SPA |
| ![CSS](https://img.shields.io/badge/CSS-1572B6?logo=css3&logoColor=white) | Стилизация интерфейса |
| ![Fetch API](https://img.shields.io/badge/Fetch_API-FF6F00?logo=javascript&logoColor=white) | Выполнение HTTP-запросов к серверу |
| ![ESLint](https://img.shields.io/badge/ESLint-4B32C3?logo=eslint&logoColor=white) | Статический анализ и контроль качества кода |

**📦 Основные зависимости:**

- `react`
- `react-dom`
- `react-router-dom`

**🧪 Dev-зависимости:**

- `vite`
- `eslint`
- `@vitejs/plugin-react`
- плагины ESLint для React

---

## 🏗️ Архитектура

Приложение построено по клиент-серверной архитектуре:

```
┌─────────────────┐       HTTP/JSON       ┌──────────────────┐
│                 │  ───────────────────▶ │                  │
│  🎨 Frontend    │                       │  ⚙️ Backend      │
│  React + Vite   │  ◀─────────────────── │  ASP.NET Core    │
│                 │                       │                  │
└─────────────────┘                       └────────┬─────────┘
                                                   │
                                                   │ EF Core
                                                   ▼
                                          ┌──────────────────┐
                                          │   🗄️ SQLite      │
                                          │    (Todo.db)     │
                                          └──────────────────┘
```

Взаимодействие между клиентом и сервером осуществляется посредством REST API с обменом данными в формате JSON. CORS-политика настроена для обеспечения кросс-доменных запросов с фронтенда.

---

## 📂 Структура проекта

```
TodoApi/
├── 📄 Program.cs                    # Точка входа, конфигурация приложения
├── 📄 TodoDb.cs                     # Модели данных и контекст EF Core
├── 📄 TodoApi.csproj                # Описание проекта и зависимостей
├── ⚙️ appsettings.json              # Основные настройки приложения
├── ⚙️ appsettings.Development.json  # Настройки для среды разработки
└── 🗄️ Todo.db                       # Файл базы данных (создаётся автоматически)
```

---

## ✅ Требования

Для запуска проекта необходимо наличие:

- 🟣 [.NET 10 SDK](https://dotnet.microsoft.com/download) или более поздней версии
- 🟢 [Node.js](https://nodejs.org/) 18+ и npm — для клиентской части
- 💻 Среда разработки: **Visual Studio 2022+**, **JetBrains Rider**, **VS Code** или другая

---

## 🚀 Установка и запуск

### ⚙️ Backend

```bash
# Клонирование репозитория
git clone https://github.com/<organization>/<repository>.git
cd <repository>

# Восстановление зависимостей
dotnet restore

# Запуск приложения
dotnet run
```

### 🎨 Frontend

```bash
# Переход в директорию клиента
cd client

# Установка зависимостей
npm install

# Запуск в режиме разработки
npm run dev

# Сборка production-версии
npm run build

# Проверка кода линтером
npm run lint
```

---

### 🌐 CORS

Политика CORS `AllowAll` разрешает запросы с любого источника, что удобно в процессе разработки. Для production-окружения рекомендуется ограничить список разрешённых источников. ⚠️

---

## 📘 Документация API

Интерактивная документация доступна через **Swagger UI** после запуска приложения.

### 📋 Основные эндпоинты

| Метод | Endpoint | Описание |
|:---:|---|---|
| 🟢 `GET` | `/api/tasks` | Получение списка задач |
| 🟢 `GET` | `/api/tasks/{id}` | Получение задачи по идентификатору |
| 🔵 `POST` | `/api/tasks` | Создание новой задачи |
| 🟡 `PUT` | `/api/tasks/{id}` | Обновление существующей задачи |
| 🔴 `DELETE` | `/api/tasks/{id}` | Удаление задачи |

### 📝 Пример запроса

```http
POST /api/tasks
Content-Type: application/json

{
  "title": "Пример задачи",
  "description": "Описание задачи",
  "userId": 1
}
```

---

## 🗃️ Модели данных

### 👤 User

Модель пользователя системы.

| Поле | Тип | Обязательное | Описание |
|---|:---:|:---:|---|
| `Id` | `int` | ✅ | Уникальный идентификатор |
| `Name` | `string?` | ❌ | Имя пользователя |
| `Email` | `string` | ✅ | Адрес электронной почты |
| `PasswordHash` | `string` | ✅ | Хеш пароля |
| `Tasks` | `List<TaskItem>` | — | Список задач пользователя |

### ✅ TaskItem

Модель задачи.

| Поле | Тип | Обязательное | Описание |
|---|:---:|:---:|---|
| `Id` | `int` | ✅ | Уникальный идентификатор |
| `Title` | `string` | ✅ | Заголовок задачи |
| `Description` | `string?` | ❌ | Описание задачи |
| `Completed` | `bool` | ✅ | Статус выполнения |
| `CreatedAt` | `DateTime` | ✅ | Дата создания |
| `UpdatedAt` | `DateTime` | ✅ | Дата последнего обновления |
| `UserId` | `int` | ✅ | Идентификатор владельца задачи |

---

## 👥 Команда

Проект разрабатывается командой в рамках курса по **продуктовому программированию**.

| 🎯 Роль | 🔗 GitHub |
|---|---|
| Backend-разработчик | [@dorothei](https://github.com/dorothei) |
| Backend-разработчик | [@shtt3red](https://github.com/shtt3red) |
| Backend-разработчик | [@bajwas3](https://github.com/bajwas3) |
| Frontend-разработчик | [@qweezy22](https://github.com/qweezy22) |
| Frontend-разработчик | [@munovsky](https://github.com/munovsky) |

---

## 📄 Лицензия

Проект распространяется под лицензией **MIT**. Подробнее см. в файле [LICENSE](LICENSE).

---

⭐ Поставьте звезду, если проект оказался полезным

</div>
