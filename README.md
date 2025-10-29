# Map-Project
ASP.NET Core ve Leaflet ile harita çizim uygulaması
-----------------------------------------------------------
@'
# 🗺️ Harita Çizim Uygulaması

ASP.NET Core, PostgreSQL ve Leaflet.js kullanılarak geliştirilmiş modern harita çizim uygulaması. Kullanıcılar harita üzerinde nokta, çizgi ve alan çizebilir, bu çizimleri veritabanında saklayabilir ve yönetebilir.

![Harita Uygulaması](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18.2-336791?logo=postgresql)
![Leaflet](https://img.shields.io/badge/Leaflet-1.9.4-199900?logo=leaflet)

## ✨ Özellikler

- 🎯 **Nokta, Çizgi ve Alan Çizimi** - Leaflet Draw ile kolay çizim
- 💾 **Veritabanı Kaydı** - Tüm çizimler PostgreSQL'de saklanır
- 📍 **GeoJSON Formatı** - Standart GeoJSON formatında kayıt
- 🎨 **Katman Yönetimi** - Çizimleri açıp kapatabilme
- 🌐 **WMS Servis Entegrasyonu** - Harita servisleri desteği
- ✏️ **Düzenleme & Silme** - Çizimleri düzenleyebilme ve silebilme
- 📱 **Responsive Tasarım** - Mobil cihazlarda uyumlu çalışır

## 🛠️ Teknolojiler

### Backend
- **.NET 9.0** - Web API Framework
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - İlişkisel Veritabanı
- **Npgsql** - PostgreSQL .NET Provider

### Frontend
- **Leaflet.js 1.9.4** - Açık Kaynak Harita Kütüphanesi
- **Leaflet Draw 1.0.4** - Çizim Eklentisi
- **HTML5, CSS3, JavaScript** - Modern Web Teknolojileri

### Veritabanı
- **PostgreSQL 16+** - Ana Veritabanı
- **GeoJSON Format** - Geometrik Veri Saklama

## 📦 Kurulum

### Ön Gereksinimler

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

### 1. Projeyi İndirme


`` `bash
# Repository'yi klonlayın
git clone https://github.com/YunusK7/Map-Project.git
cd Map-Project
----------------------------------------------------------------

2. Veritabanı Kurulumu
A. PostgreSQL Kurulumu
PostgreSQL'i yükleyin ve başlatın

pgAdmin veya psql ile veritabanı oluşturun:

--------SQL------------
-- Yeni veritabanı oluştur
CREATE DATABASE map_project;

-- Tabloyu oluştur
CREATE TABLE "GeoEntities" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT,
    "GeometryType" TEXT NOT NULL,
    "GeoJson" TEXT NOT NULL,
    "Color" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL
);

-- Test verisi ekle (opsiyonel)
INSERT INTO "GeoEntities" ("Name", "Description", "GeometryType", "GeoJson", "Color", "CreatedAt", "UpdatedAt")
VALUES 
    ('Örnek Nokta', 'İlk test noktası', 'Point', '{"type":"Point","coordinates":[32.8597,39.9334]}', '#ff0000', NOW(), NOW());


B. Bağlantı Ayarları
appsettings.json dosyasını düzenleyin:

appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=map_project;Username=postgres;Password=sizin_şifreniz;Port=5432"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
3. Uygulamayı Çalıştırma
Terminal:
# Gerekli paketleri yükle
dotnet restore

# Uygulamayı başlat
dotnet run

# Veya development modunda
dotnet run --environment Development
Uygulama varsayılan olarak http://localhost:5063 adresinde çalışacaktır.

🚀 Kullanım
Uygulamayı Başlatın: dotnet run

Tarayıcıda Açın: http://localhost:5063

Çizim Yapın:

📍 Nokta: Marker butonuna tıklayın, haritaya tıklayın

📏 Çizgi: Polyline butonuna tıklayin, noktaları tıklayın, çift tıklayarak bitirin

🔷 Alan: Polygon butonuna tıklayın, noktaları tıklayın, çift tıklayarak bitirin

Kaydedin: Çizim tamamlandığında isim ve açıklama girip kaydedin

🗂️ Proje Yapısı
MapProject/
├── Controllers/
│   └── GeoEntitiesController.cs     # API Endpoints
├── Models/
│   └── GeoEntity.cs                 # Veri Modeli
├── Data/
│   └── ApplicationDbContext.cs      # DbContext
├── Migrations/                      # Database Migrations
├── wwwroot/
│   └── index.html                   # Frontend Arayüz
├── Properties/
│   └── launchSettings.json          # Çalışma Ayarları
├── Program.cs                       # Uygulama Giriş Noktası
├── appsettings.json                 # Yapılandırma

🌐 API Dokümantasyonu
Endpoints
Method	      Endpoint	               Açıklama
GET	         /api/geoentities	        Tüm geometrileri listele
POST	       /api/geoentities	        Yeni geometri ekle
PUT	         /api/geoentities/{id}	  Geometri güncelle
DELETE	     /api/geoentities/{id}	  Geometri sil

Örnek API Kullanımı:
# Tüm geometrileri getir
curl -X GET http://localhost:5063/api/geoentities

# Yeni nokta ekle
curl -X POST http://localhost:5063/api/geoentities \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Yeni Nokta",
    "description": "API ile eklendi",
    "geoJson": "{\"type\":\"Point\",\"coordinates\":[32.8597,39.9334]}"
  }'


🔧 Geliştirme
Database Migration
# Yeni migration oluştur
dotnet ef migrations add MigrationAdi

# Database'i güncelle
dotnet ef database update
Frontend Geliştirme
Frontend dosyaları wwwroot/ klasöründe bulunur. Değişiklik yaptıktan sonra uygulamayı yeniden başlatmanız gerekebilir.

🐛 Sorun Giderme
Sık Karşılaşılan Sorunlar
1.Database Bağlantı Hatası

-PostgreSQL'in çalıştığından emin olun
-Connection string'i kontrol edin
-Veritabanının var olduğundan emin olun

2.Port Çakışması
# Mevcut dotnet proseslerini durdur
taskkill /f /im dotnet.exe

# Farklı portta çalıştır
dotnet run --urls="http://localhost:5000"

3.CORS Hatası

Program.cs dosyasında CORS ayarlarını kontrol edin

👨‍💻 Geliştirici
YunusK7 - GitHub
📞 İletişim & Destek
Repository: https://github.com/YunusK7/Map-Project
e-posta: kanburyunusemre28@gmail.com 

Sorularınız için: GitHub Issues kısmından soru oluşturabilirsiniz

⭐ Bu projeyi beğendiyseniz GitHub'da yıldız vermeyi unutmayın!
