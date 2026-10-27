<div align="center">

# 💳 FinanceTracker

**От учебного банкомата — к приложению личных финансов**

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![C%23](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4)
![Status](https://img.shields.io/badge/status-lab_5_baseline-blue)

Счета · Операции · Сессии · История транзакций

</div>

---

## О проекте

FinanceTracker начинается с пятой лабораторной работы по объектно-ориентированному программированию: HTTP API учебного банкомата на C#. Этот репозиторий содержит исходную бизнес-логику лабораторной и её тесты. Учёт расходов, бюджеты и аналитика пока находятся в планах.

При выделении проекта добавлен отдельный solution, отключена повторная компиляция вложенных исходников в агрегирующем `Lab5.csproj` и убрана установка Git-хуков исходного учебного репозитория. Прикладной код не изменён.

## Что уже реализовано

- Создание пользовательской сессии по номеру счёта и PIN-коду.
- Создание административной сессии по системному паролю.
- Проверка существования сессии.
- Просмотр баланса, пополнение и снятие средств.
- Проверка положительной суммы и достаточности средств.
- История операций по счёту.
- Swagger UI для знакомства с API.
- Пять unit-тестов операций со счётом на xUnit и NSubstitute.

## Архитектура

```text
HTTP request
     │
     ▼
Presentation ─────► Application ─────► DomainModel
                         ▲
                         │ interfaces
                   Infrastructure
                   (in-memory storage)
```

| Слой | Ответственность |
| :--- | :--- |
| `DomainModel` | Счёт, сессия, транзакция и предметные исключения |
| `Application` | Сценарии операций и интерфейсы репозиториев |
| `Infrastructure` | Хранение счетов, сессий и операций в памяти |
| `Presentation` | Контроллеры, модели запросов и настройка ASP.NET Core |

## Быстрый старт

Нужен **.NET SDK 9.0.3xx** или совместимый SDK с поддержкой `.slnx` и целевой платформы `net9.0`. Команды выполняются из корня репозитория. При первом запуске требуется доступ к NuGet.

```bash
dotnet restore FinanceTracker.slnx
dotnet build FinanceTracker.slnx
dotnet run --project src/Lab5/Presentation/Presentation.csproj --launch-profile http
```

Swagger UI: **http://localhost:5163/swagger**. Профиль `http` включает окружение Development, в котором доступен Swagger.

Для HTTPS-профиля:

```bash
dotnet dev-certs https --trust
dotnet run --project src/Lab5/Presentation/Presentation.csproj --launch-profile https
```

Swagger UI: **https://localhost:7233/swagger**.

### Первый запрос

Системный пароль читается из параметра `SystemPassword`; если он не задан, используется учебное значение `admin`. Его можно переопределить переменной окружения `SystemPassword` перед запуском приложения.

```bash
curl -X POST http://localhost:5163/api/Session/create-admin \
  -H 'Content-Type: application/json' \
  -d '{"systemPassword":"admin"}'
```

Ответ содержит `sessionKey`. Проверить сессию:

```bash
curl -X POST http://localhost:5163/api/Session/validate \
  -H 'Content-Type: application/json' \
  -d '{"sessionKey":"PASTE_SESSION_KEY_HERE"}'
```

**Особенность исходной лабораторной:** хранилище счетов изначально пустое, а HTTP-метода создания счёта и демонстрационных данных пока нет. Для пользовательских операций сначала нужен счёт, сохранённый через `IAccountRepository.Save` в коде инициализации или теста. Административная сессия не создаёт пользовательский счёт.

## API

Все перечисленные маршруты используют `POST`; параметры передаются в JSON-теле.

| Маршрут | Назначение |
| :--- | :--- |
| `/api/Session/create-user` | Вход по номеру счёта и PIN-коду |
| `/api/Session/create-admin` | Административная сессия |
| `/api/Session/validate` | Проверка сессии |
| `/api/Account/balance` | Баланс счёта |
| `/api/Account/deposit` | Пополнение |
| `/api/Account/withdraw` | Снятие средств |
| `/api/Transaction/history` | История операций |

Точные схемы запросов и ответов доступны в Swagger UI.

## Тесты

```bash
dotnet test FinanceTracker.slnx
```

Существующие тесты проверяют изменение баланса при пополнении и снятии, отказ при недостаточном балансе и создание записей об операциях. Репозитории подменяются с помощью NSubstitute; база данных для тестов не нужна.

## Структура

```text
FinanceTracker/
├── FinanceTracker.slnx
├── Directory.Build.props
├── Directory.Packages.props
├── src/Lab5/
│   ├── Lab5.csproj           # Агрегирующий проект
│   ├── DomainModel/
│   ├── Application/
│   ├── Infrastructure/
│   └── Presentation/
└── tests/Lab5.Tests/
```

## Ограничения текущей версии

Данные хранятся только в памяти и пропадают после перезапуска. Сессии не имеют срока действия; PIN хранится без хеширования. Изменение баланса и запись транзакции выполняются отдельно, а конкурентные операции не защищены. Это учебная стартовая версия для дальнейшего развития.

## Дальнейшее развитие

- [ ] Создание счетов и демонстрационные данные.
- [ ] PostgreSQL, EF Core и миграции.
- [ ] Авторизация, хеширование секретов и срок действия сессий.
- [ ] Атомарные операции и защита от конкурентных изменений.
- [ ] Доходы, расходы, категории и переводы между кошельками.
- [ ] Бюджеты и статистика за месяц.
- [ ] Веб-интерфейс, Docker Compose и CI.

---

<div align="center">
Основа — лабораторная №5 из репозитория TemaStarkov по ООП.
</div>
