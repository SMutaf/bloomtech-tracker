using BloomTech.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloomTech.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Entities
        public DbSet<Company> Companies { get; set; }
        public DbSet<StockData> StockData { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<InsiderTrade> InsiderTrades { get; set; } // Yeni
        public DbSet<SupplyChain> SupplyChains { get; set; }   // Yeni

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- PDF MİMARİSİNE UYGUN TABLO İSİMLERİ ---

            // PDF[cite: 113]: Tablo adı "Stocks"
            modelBuilder.Entity<Company>().ToTable("Stocks");

            // PDF[cite: 114]: Tablo adı "PriceHistory"
            modelBuilder.Entity<StockData>().ToTable("PriceHistory");

            // PDF[cite: 115]: Tablo adı "News"
            modelBuilder.Entity<News>().ToTable("News");

            // InsiderTrades ve SupplyChain için entity içindeki [Table] attribute'u yeterli.

            // Constraints (Kısıtlamalar)
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Symbol)
                .IsUnique();
        }
    }
}