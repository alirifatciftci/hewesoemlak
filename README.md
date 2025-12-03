# Emlak Yönetim Sistemi

ASP.NET Core MVC ile geliştirilmiş profesyonel emlak yönetim platformu.

## Özellikler

- ✅ **Kiracı Yönetimi**: Multi-tenant yapı ile kiracı yönetimi
- ✅ **Emlak Yönetimi**: Emlak ekleme, düzenleme, silme ve listeleme
- ✅ **Müşteri Yönetimi**: Müşteri bilgileri ve talepleri yönetimi
- ✅ **Randevu Yönetimi**: Emlak görüntüleme randevuları
- ✅ **RBAC (Role-Based Access Control)**: Kullanıcı rolleri ve yetkileri
- ✅ **Modern UI**: Bootstrap ile responsive tasarım

## Teknolojiler

- .NET 9.0
- ASP.NET Core MVC
- Entity Framework Core 9.0
- SQL Server
- Bootstrap 5

## Veritabanı

Proje SQL Server veritabanı kullanmaktadır. Veritabanı şeması için SQL script dosyasına bakınız.

### Connection String Yapılandırması

`appsettings.json` dosyasında connection string'i kendi SQL Server bilgilerinize göre güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EmlakYonetimDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**Not:** Geliştirme ortamı için gerçek connection string `appsettings.Development.json` dosyasında saklanmaktadır (bu dosya Git'e commit edilmez).

## Kurulum

1. Projeyi klonlayın:
```bash
git clone https://github.com/alirifatciftci/hewesoemlak.git
cd hewesoemlak
```

2. NuGet paketlerini yükleyin:
```bash
dotnet restore
```

3. `appsettings.json` dosyasındaki connection string'i güncelleyin

4. Veritabanını oluşturun (SQL script'i çalıştırın)

5. Uygulamayı çalıştırın:
```bash
dotnet run
```

## Proje Yapısı

```
EmlakYonetim/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── KiracilarController.cs
│   ├── EmlaklarController.cs
│   └── MusterilerController.cs
├── Models/              # Entity Models
│   ├── Kiraci.cs
│   ├── Emlak.cs
│   ├── Musteri.cs
│   └── ...
├── Data/                # DbContext
│   └── EmlakYonetimDbContext.cs
├── Views/               # Razor Views
│   ├── Home/
│   ├── Kiracilar/
│   ├── Emlaklar/
│   └── Musteriler/
└── wwwroot/             # Static Files
```

## Veritabanı Şeması

- **Kiracilar**: Multi-tenant yapı için kiracı bilgileri
- **Kullanicilar**: Sistem kullanıcıları (danışmanlar, yöneticiler)
- **Roller & Yetkiler**: RBAC yapısı
- **Musteriler**: Emlak alıcıları/satıcıları
- **Emlaklar**: Emlak ilanları
- **Randevular**: Emlak görüntüleme randevuları
- **EmlakDurumlari**: Satılık/Kiralık durumları
- **EmlakTipleri**: Daire/Villa/Arsa gibi tipler

## Lisans

Bu proje özel bir projedir.

