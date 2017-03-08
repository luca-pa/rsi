using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportYahooFinance
{
    public class TradingContext : DbContext
    {
        const string connectionString = @"server=(local)\sqlexpress;database=Trading;trusted_connection=true;";

        public DbSet<Quota> Quote { get; set; }
        public DbSet<Etf> Etfs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quota>()
                .ToTable("Quote")
                .HasKey(q => new { q.Ticker, q.Data });

            modelBuilder.Entity<Etf>()
                .ToTable("ETFs")
                .HasKey("Ticker");
        }

    }
}
