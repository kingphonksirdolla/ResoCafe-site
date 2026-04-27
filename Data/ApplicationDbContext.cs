using Microsoft.EntityFrameworkCore;
using ResoCafe.Models;

namespace ResoCafe.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasDefaultValue("user");
        });

        builder.Entity<MenuItem>(e =>
        {
            e.Property(m => m.Price).HasPrecision(10, 2);
        });

        builder.Entity<Order>(e =>
        {
            e.Property(o => o.Total).HasPrecision(10, 2);
        });

        builder.Entity<OrderItem>(e =>
        {
            e.Property(o => o.Price).HasPrecision(10, 2);
        });

        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        // пароль админа: ResoAdmin#2025
        const string adminHash = "$2b$12$Wfp2gNKHpg/g582i5UG5c.Fc0EKMtzQKSHDvMqlhPTdybiNAIT3cW";

        builder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@resocafe.ru",
            PasswordHash = adminHash,
            Role = "admin",
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        builder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Category = "Горячий кофе",   Name = "Капучино",        Description = "Эспрессо с молоком и обильной пеной",       Price = 200 },
            new MenuItem { Id = 2, Category = "Эспрессо",        Name = "Эспрессо",        Description = "Насыщенный, 50 мл",                          Price = 150 },
            new MenuItem { Id = 3, Category = "Горячий кофе",   Name = "Американо",       Description = "Эспрессо с горячей водой",                   Price = 180 },
            new MenuItem { Id = 4, Category = "Горячий кофе",   Name = "Латте",           Description = "Много молока, нежная пенка",                 Price = 220 },
            new MenuItem { Id = 5, Category = "Авторский кофе", Name = "Раф-кофе",        Description = "Кофе со сливками и ванилью",                 Price = 250 },
            new MenuItem { Id = 6, Category = "Напитки",        Name = "Горячий шоколад", Description = "Густой, с маршмеллоу",                       Price = 200 },
            new MenuItem { Id = 7, Category = "Выпечка",        Name = "Круассан",        Description = "Классический, мягкая маслянистая текстура",  Price = 120 },
            new MenuItem { Id = 8, Category = "Десерты",        Name = "Чизкейк",         Description = "Сливочный, с ягодным соусом",                Price = 220 }
        );
    }
}
