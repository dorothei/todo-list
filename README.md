# 🚀 TodoList

**Приложение для управления задачами**, разработанное в рамках командного проекта по продуктовому программированию. Проект включает серверную часть (REST API) и клиентское приложение на React.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)

![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![Vite](https://img.shields.io/badge/Vite-646CFF?style=for-the-badge&logo=vite&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![React Router](https://img.shields.io/badge/React_Router-CA4245?style=for-the-badge&logo=reactrouter&logoColor=white)
![ESLint](https://img.shields.io/badge/ESLint-4B32C3?style=for-the-badge&logo=eslint&logoColor=white)

![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-In_Development-yellow?style=for-the-badge)

## 🎯 О проекте

**TodoList** — приложение для управления задачами. Проект состоит из двух независимых частей:

- **Backend** — REST API на ASP.NET Core, хранит пользователей и их задачи в SQLite, предоставляет интерактивную документацию через Swagger.
- **Frontend** — SPA-приложение на React + Vite, обеспечивающее удобный интерфейс для работы с задачами: список, создание, редактирование, удаление и отметка о выполнении.

Проект разрабатывается командой в рамках учебного курса по продуктовому программированию.

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
| ![React Router](https://img.shields.io/badge/React_Router-CA4245?logo=reactrouter&logoColor=white) | Клиентская маршрутизация в SPA |
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

- **Frontend** общается с backend через `Fetch API`, получая и отправляя JSON.
- **Backend** обрабатывает запросы, валидирует данные и через `EF Core` работает с базой `SQLite`.

---

## 📂 Структура проекта

```
TodoList/
│
├── backend/                          # 🔧 Серверная часть
│   ├── Program.cs                    # Точка входа, конфигурация приложения
│   ├── TodoDb.cs                     # Модели данных и контекст EF Core
│   ├── TodoApi.csproj                # Описание проекта и зависимостей
│   ├── appsettings.json              # Основные настройки
│   ├── appsettings.Development.json  # Настройки для среды разработки
│   └── Todo.db                       # Файл базы данных (создаётся автоматически)
│
└── client/                           # 🎨 Клиентская часть
    ├── public/                       # Статические ресурсы
    ├── src/
    │   ├── components/               # Переиспользуемые React-компоненты
    │   ├── pages/                    # Страницы приложения (роуты)
    │   ├── api/                      # Обёртки над Fetch API для работы с backend
    │   ├── styles/                   # CSS-стили
    │   ├── App.jsx                   # Корневой компонент, роутинг
    │   └── main.jsx                  # Точка входа Vite
    ├── index.html                    # HTML-шаблон
    ├── package.json                  # Зависимости и скрипты
    ├── vite.config.js                # Конфигурация Vite
    └── .eslintrc.cjs                 # Конфигурация ESLint
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
# Переход в директорию backend
cd backend

# Восстановление зависимостей
dotnet restore

# Запуск приложения
dotnet run
```

Файл базы данных `Todo.db` создаётся автоматически при первом запуске.

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

# Предпросмотр production-сборки
npm run preview

# Проверка кода линтером
npm run lint
```

> ⚠️ **Важно:** для полноценной работы интерфейса backend должен быть запущен параллельно — клиент обращается к API по адресу из конфигурации.

---

## 🎨 Клиентская часть

### Что реализовано

- 🗂️ **Маршрутизация** через `React Router DOM` — переходы между страницами без перезагрузки
- 📋 **Просмотр списка задач** — получение данных с backend через `Fetch API`
- ➕ **Создание задач** — форма с отправкой `POST`-запроса
- ✏️ **Редактирование задач** — обновление через `PUT`-запрос
- ✅ **Отметка выполнения** — переключение статуса задачи
- 🗑️ **Удаление задач** — `DELETE`-запрос с обновлением списка
- 🎨 **Стилизация** на чистом CSS — без сторонних UI-библиотек
- 🧹 **Линтинг** через `ESLint` с плагинами для React

---

## 📘 Документация API

Интерактивная документация доступна через **Swagger UI**.

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
{
  "title": "string",
  "description": "string",
  "completed": true
}

GET /api/tasks/{id}
{
  "id": 0,
  "title": "string",
  "description": "string",
  "completed": true,
  "updatedAt": "2026-09-30T18:59:24.712Z"
}

PUT /api/tasks/{id}
{
  "title": "string",
  "description": "string",
  "completed": true
}
```

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
