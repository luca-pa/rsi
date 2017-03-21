using Microsoft.EntityFrameworkCore;
using RSI.Models;

namespace RSI.Repositories
{
    public class TradingContext : DbContext
    {
        const string connectionString = @"server=(local)\sqlexpress;database=Trading;trusted_connection=true;";

        public DbSet<Quota> Quote { get; set; }
        public DbSet<QuotaPortafoglio> QuotePortafoglio { get; set; }
        public DbSet<Etf> Etfs { get; set; }
        public DbSet<Selezione> Selezione { get; set; }
        public DbSet<PortafoglioItem> PortafoglioItems { get; set; }
        public DbSet<Bilancio> Bilancio { get; set; }
        public DbSet<StoricoItem> StoricoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quota>(t =>
            {
                t.ToTable("Quote")
                    .HasKey(q => new { q.Ticker, q.Data });
                t.HasOne(q => q.Etf)
                    .WithMany(e => e.Quote)
                    .HasForeignKey(e => e.Ticker);
                t.Ignore(q => q.Variazione);
                t.Ignore(q => q.ChiusuraPrecedente);
            });

            modelBuilder.Entity<Etf>(t =>
            {
                t.ToTable("ETFs")
                    .HasKey("Ticker");
                t.HasOne<Selezione>()
                    .WithOne(s => s.Etf)
                    .HasForeignKey<Selezione>(s => s.Ticker);
                t.HasOne<PortafoglioItem>()
                    .WithOne(p => p.Etf)
                    .HasForeignKey<PortafoglioItem>(p => p.Ticker);
                t.HasMany<Quota>(e => e.Quote)
                    .WithOne(q => q.Etf)
                    .HasForeignKey(q => q.Ticker);
                t.Ignore(e => e.IsOwned);
            });

            modelBuilder.Entity<Selezione>(t =>
            {
                t.ToTable("Selezione")
                    .HasKey("Ticker");
                t.Property(s => s.Ticker)
                    .HasColumnName("Ticker");
                t.HasOne<Etf>()
                    .WithOne(e => e.Selezione)
                    .HasForeignKey<Etf>(e => e.Ticker);
            });

            modelBuilder.Entity<PortafoglioItem>(t =>
            {
                t.ToTable("Portafoglio")
                    .HasKey("Ticker", "Data");
                t.Property(s => s.Ticker)
                    .HasColumnName("Ticker");
                t.Ignore(s => s.PrezzoCorrente);
                t.Ignore(s => s.Variazione);
            });

            modelBuilder.Entity<QuotaPortafoglio>(t =>
            {
                t.ToTable("QuotePortafoglio")
                    .HasKey(q => new { q.Ticker, q.Data });
            });

            modelBuilder.Entity<Bilancio>(t =>
            {
                t.ToTable("Bilancio")
                    .HasKey("Data");
            });

            modelBuilder.Entity<StoricoItem>(t =>
            {
                t.HasKey(i => i.Data);
            });

        }

    }
}
