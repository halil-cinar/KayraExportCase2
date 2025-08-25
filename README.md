# Kayra Export Case 2 - Backend Developer Task

## 📌 Proje Hakkında
Bu proje, **Kayra Export** şirketinin mülakat case çalışmasıdır. Proje; **.NET Core Web API**, **PostgreSQL**, **Redis**, **EF Core**, **JWT Authentication** ve **Onion Architecture** kullanılarak geliştirilmiştir. Amaç, ölçeklenebilir servis mantığı, kimlik doğrulama, caching ve temel CQRS kullanımını göstermek ve değerlendirmektir.

## ⚙️ Teknik Gereksinimler
- .NET 7+ (ASP.NET Core Web API)
- C#
- Entity Framework Core (EF Core)
- PostgreSQL
- Redis (cache)
- JWT Authentication
- Swagger (API dokümantasyonu)
- Onion Architecture (Core – Application – Infrastructure – API)
- Migration yönetimi
- Global Exception Handling & Logging (Console veya Serilog)

## 📂 Proje Mimarisi (Onion Architecture)
```
+---KayraExportCase2.API
|   +---Controllers        # API controller katmanı
|   +---Middlewares        # Exception, Logging vb. global middleware'ler
|   +---Properties
|
+---KayraExportCase2.Application
|   +---Abstract           # Service interface tanımları
|   +---Caching            # Redis cache implementasyonları
|   +---Result             # SystemResult tipleri
|   +---Services           # İş mantığı servisleri
|
+---KayraExportCase2.DataAccess
|   +---Abstract           # Repository interface tanımları
|   +---Context            # DbContext ve konfigürasyon
|   +---Migrations         # EF Core migration dosyaları
|
+---KayraExportCase2.Domain
    +---Abstract           # Domain interface tanımları
    +---Dtos               # Veri transfer objeleri
    +---Entities           # Veritabanı tabloları
    +---ListDtos           # Listeleme için kullanılan DTO’lar
```

## 🔑 Servisler

### Auth Servisi
- **POST /api/Auth/signup** → Kullanıcı kayıt
- **POST /api/Auth/login** → Login (JWT üretimi)

### Product Servisi
- **POST /api/Product** → Ürün oluşturur.
- **GET /api/Product** → Sayfalı ürün listesi döner. (Redis Cache kullanılır)
- **PUT /api/Product/{id}** → Ürün günceller (Cache invalidation yapılır).
- **DELETE /api/Product/{id}** → Ürün siler (Cache invalidation yapılır).
- **GET /api/Product/{id}** → Tekil ürün getirir.

## 🚀 Kurulum ve Çalıştırma

### 1. Gereksinimler
- PostgreSQL Sunucusu
- Redis Sunucusu

### 2. Connection String Ayarları
`appsettings.json` dosyası içinde:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=...;Database=...;Username=...;Password=...;"
},
"Cache": {
  "RedisConnectionString": "localhost:6380",
  "InstanceName": "KayraExport_"
}
```
> Not: Proje, test ortamında **WSL üzerinde Redis** çalıştırılmıştır. Port çakışmasından dolayı **6379 yerine 6380** kullanılmaktadır. Eğer mevcut PostgreSQL sunucusu çalışmazsa, kendi sunucunuzun yolunu güncelleyin.

### 3. Migration Uygulama
```bash
dotnet ef database update --project KayraExportCase2.DataAccess
```

### 4. Projeyi Çalıştırma
```bash
dotnet run --project KayraExportCase2.API
```
Çalıştıktan sonra Swagger arayüzüne erişebilirsiniz:
```
http://localhost:5000/swagger
```

## 🛠 Teknik Beklentiler
- JWT ile kimlik doğrulama
- Redis cache entegrasyonu
- CQRS Pattern kullanımı
- SOLID prensiplerine uygun servis yapısı
- Migration yönetimi
- Global exception handling + logging


---
📌 **Not:** Bu proje yalnızca Kayra Export mülakat case çalışması için hazırlanmıştır.
