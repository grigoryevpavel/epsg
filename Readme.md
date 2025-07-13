# Проект EPSG 

## Обзор
Этот проект расчета координат по системе EPSG для управления сельскохозяйственными полями, их расположением и геометрией. 
API использует PostgreSQL для хранения данных и предоставляет эндпоинты для получения информации о полях, 
вычисления площади, расстояния и проверки принадлежности точки контуру поля. Для интерактивного тестирования доступен Swagger UI.

## Технологии
- ASP.NET Core (.NET 9)
- Entity Framework Core (Npgsql)
- PostgreSQL
- Swagger/OpenAPI

## Быстрый старт
1. Укажите строку подключения к PostgreSQL в `appsettings.json`:
   ```json
   "DefaultConnection": "Host=pgserver;Port=5432;Database=fieldsdb;Username=epsg;Password=123456"
   ```
2. Проведите миграции базы данных:
   ```sh
   dotnet ef database update --project EPSG.API
   ```
3. Запустите API:
   ```sh
   dotnet run --project EPSG.API
   ```
4. Откройте Swagger UI в режиме разработки в браузере: [http://localhost:32773/](http://localhost:32773/)

## Эндпоинты (FieldsController)

### Получить все поля
- **GET** `/fields/all`
- Возвращает все поля с параметрами: `id`, `name`, `size`, `locations` (центр, полигон).

### Получить площадь поля
- **GET** `/fields/size/{id}`
- Возвращает площадь (`size`) поля по идентификатору.

### Получить расстояние до точки
- **GET** `/fields/distance?id={id}&lat={lat}&lng={lng}`
- Возвращает расстояние в метрах от центра поля до указанной точки.

### Проверить принадлежность точки полю
- **GET** `/fields/contains?lat={lat}&lng={lng}`
- Проверяет, находится ли точка внутри контура какого-либо поля.
- Возвращает `{ id, name }` если найдено, иначе `false`.

## Модель данных
- **Field**: `Id`, `Name`, `Size`, `Locations`
- **Locations**: `Id`, `Center` (массив широта/долгота), `Polygon` (список точек)
- **LocationPoint**: `Id`, `Lat`, `Lng`

## Swagger & OpenAPI
- Все эндпоинты документированы и доступны для тестирования через Swagger UI.
- OpenAPI спецификация доступна по адресу `/swagger/v1/swagger.json`.

## Лицензия
MIT
