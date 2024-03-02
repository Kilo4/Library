using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Webapi.Models;

namespace Webapi.Models.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Order>()
        //     .Property(p => p.OrderDate)
        //     .HasDefaultValueSql("CURRENT_TIMESTAMP");
        // modelBuilder.Entity<Order>()
        //     .Property(p => p.LastDate)
        //     .HasDefaultValueSql("CURRENT_TIMESTAMP + INTERVAL '30 days'");
        // modelBuilder.Entity<Order>()
        //     .Property(b => b.Books)
        //     .HasConversion(
        //         v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        //         v => JsonConvert.DeserializeObject<List<Book>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        //
        // modelBuilder.Entity<Book>()
        //     .HasIndex(b => new { b.Author, b.Title })
        //     .IsUnique();
    }
    public DbSet<Article> Article { get; set; } = null!;
    public DbSet<Author> Author { get; set; } = null!;
    public DbSet<Image> Image { get; set; } = null!;
    public DbSet<Site> Site { get; set; } = null!;
    
}