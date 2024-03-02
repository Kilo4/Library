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
        modelBuilder.Entity<Image>().HasKey(key => key.Id);
        
        modelBuilder.Entity<Site>().HasKey(key => key.Id);
        modelBuilder.Entity<Site>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<Author>().HasKey(key => key.Id);
        modelBuilder.Entity<Author>().HasIndex(p => p.Name).IsUnique();
        modelBuilder.Entity<Author>().HasOne(a => a.Image)
            .WithOne()
            .HasForeignKey<Image>(a => a.Id)
            .IsRequired(true);

        modelBuilder.Entity<Article>().HasKey(key => key.Id);
        modelBuilder.Entity<Article>().HasIndex(key => key.Title);
        modelBuilder.Entity<Article>()
            .HasOne(e => e.Site)
            .WithMany()
            .HasForeignKey("SiteId")
            .HasPrincipalKey(nameof(Article.Id))
            .IsRequired(true);
        modelBuilder.Entity<Article>()
            .HasMany(a => a.Author)
            .WithMany()
            .UsingEntity(
                "ArticleAuthor",
                l => l.HasOne(typeof(Author)).WithMany().HasForeignKey("ArticleId").HasPrincipalKey(nameof(Author.Id)),
                r => r.HasOne(typeof(Article)).WithMany().HasForeignKey("AuthorId").HasPrincipalKey(nameof(Article.Id))
                );
        
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<Site> Sites { get; set; } = null!;
    
}