using Microsoft.EntityFrameworkCore;
using EmlakYonetim.Models;

namespace EmlakYonetim.Data;

public class EmlakYonetimDbContext : DbContext
{
    public EmlakYonetimDbContext(DbContextOptions<EmlakYonetimDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<EmlakDurumu> EmlakDurumlari { get; set; }
    public DbSet<EmlakTipi> EmlakTipleri { get; set; }
    public DbSet<Kiraci> Kiracilar { get; set; }
    public DbSet<Rol> Roller { get; set; }
    public DbSet<Yetki> Yetkiler { get; set; }
    public DbSet<RolYetki> RolYetkileri { get; set; }
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Musteri> Musteriler { get; set; }
    public DbSet<Emlak> Emlaklar { get; set; }
    public DbSet<Randevu> Randevular { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EmlakDurumlari - Unique constraint
        modelBuilder.Entity<EmlakDurumu>()
            .HasIndex(e => e.DurumAd)
            .IsUnique();

        // EmlakTipleri - Unique constraint
        modelBuilder.Entity<EmlakTipi>()
            .HasIndex(e => e.TipAd)
            .IsUnique();

        // Kiracilar - Unique constraint
        modelBuilder.Entity<Kiraci>()
            .HasIndex(e => e.FirmaAd)
            .IsUnique();

        // Roller - Unique constraint
        modelBuilder.Entity<Rol>()
            .HasIndex(e => e.RolAd)
            .IsUnique();

        // Yetkiler - Unique constraint
        modelBuilder.Entity<Yetki>()
            .HasIndex(e => e.YetkiKod)
            .IsUnique();

        // Kullanicilar - Unique constraint
        modelBuilder.Entity<Kullanici>()
            .HasIndex(e => e.Email)
            .IsUnique();

        // Musteriler - Unique constraint (Email nullable olduğu için sadece null olmayanlar için)
        modelBuilder.Entity<Musteri>()
            .HasIndex(e => e.Email)
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL");

        // RolYetkileri - Composite Primary Key
        modelBuilder.Entity<RolYetki>()
            .HasKey(r => new { r.RolID, r.YetkiID });

        // Randevu - Durum Check Constraint
        modelBuilder.Entity<Randevu>()
            .Property(r => r.Durum)
            .HasConversion<string>();

        // Foreign Key Relationships
        modelBuilder.Entity<Kullanici>()
            .HasOne(k => k.Kiraci)
            .WithMany(k => k.Kullanicilar)
            .HasForeignKey(k => k.KiraciID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Kullanici>()
            .HasOne(k => k.Rol)
            .WithMany(r => r.Kullanicilar)
            .HasForeignKey(k => k.RolID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Musteri>()
            .HasOne(m => m.Kiraci)
            .WithMany(k => k.Musteriler)
            .HasForeignKey(m => m.KiraciID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emlak>()
            .HasOne(e => e.Kiraci)
            .WithMany(k => k.Emlaklar)
            .HasForeignKey(e => e.KiraciID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emlak>()
            .HasOne(e => e.Danisman)
            .WithMany(k => k.Emlaklar)
            .HasForeignKey(e => e.DanismanID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emlak>()
            .HasOne(e => e.Durum)
            .WithMany(d => d.Emlaklar)
            .HasForeignKey(e => e.DurumID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Emlak>()
            .HasOne(e => e.Tip)
            .WithMany(t => t.Emlaklar)
            .HasForeignKey(e => e.TipID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Randevu>()
            .HasOne(r => r.Emlak)
            .WithMany(e => e.Randevular)
            .HasForeignKey(r => r.EmlakID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Randevu>()
            .HasOne(r => r.Musteri)
            .WithMany(m => m.Randevular)
            .HasForeignKey(r => r.MusteriID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Randevu>()
            .HasOne(r => r.Danisman)
            .WithMany(k => k.Randevular)
            .HasForeignKey(r => r.DanismanID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolYetki>()
            .HasOne(ry => ry.Rol)
            .WithMany(r => r.RolYetkileri)
            .HasForeignKey(ry => ry.RolID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolYetki>()
            .HasOne(ry => ry.Yetki)
            .WithMany(y => y.RolYetkileri)
            .HasForeignKey(ry => ry.YetkiID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

