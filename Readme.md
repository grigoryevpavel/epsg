# ������ EPSG 

## �����
���� ������ ������� ��������� �� ������� EPSG ��� ���������� ��������������������� ������, �� ������������� � ����������. 
API ���������� PostgreSQL ��� �������� ������ � ������������� ��������� ��� ��������� ���������� � �����, 
���������� �������, ���������� � �������� �������������� ����� ������� ����. ��� �������������� ������������ �������� Swagger UI.

## ����������
- ASP.NET Core (.NET 9)
- Entity Framework Core (Npgsql)
- PostgreSQL
- Swagger/OpenAPI

## ������� �����
1. ������� ������ ����������� � PostgreSQL � `appsettings.json`:
   ```json
   "DefaultConnection": "Host=pgserver;Port=5432;Database=fieldsdb;Username=epsg;Password=123456"
   ```
2. ��������� �������� ���� ������:
   ```sh
   dotnet ef database update --project EPSG.API
   ```
3. ��������� API:
   ```sh
   dotnet run --project EPSG.API
   ```
4. �������� Swagger UI � ������ ���������� � ��������: [http://localhost:32773/](http://localhost:32773/)

## ��������� (FieldsController)

### �������� ��� ����
- **GET** `/fields/all`
- ���������� ��� ���� � �����������: `id`, `name`, `size`, `locations` (�����, �������).

### �������� ������� ����
- **GET** `/fields/size/{id}`
- ���������� ������� (`size`) ���� �� ��������������.

### �������� ���������� �� �����
- **GET** `/fields/distance?id={id}&lat={lat}&lng={lng}`
- ���������� ���������� � ������ �� ������ ���� �� ��������� �����.

### ��������� �������������� ����� ����
- **GET** `/fields/contains?lat={lat}&lng={lng}`
- ���������, ��������� �� ����� ������ ������� ������-���� ����.
- ���������� `{ id, name }` ���� �������, ����� `false`.

## ������ ������
- **Field**: `Id`, `Name`, `Size`, `Locations`
- **Locations**: `Id`, `Center` (������ ������/�������), `Polygon` (������ �����)
- **LocationPoint**: `Id`, `Lat`, `Lng`

## Swagger & OpenAPI
- ��� ��������� ��������������� � �������� ��� ������������ ����� Swagger UI.
- OpenAPI ������������ �������� �� ������ `/swagger/v1/swagger.json`.

## ��������
MIT
