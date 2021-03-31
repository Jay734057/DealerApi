using Microsoft.EntityFrameworkCore;
using TaskV3.Core.Models;

namespace TaskV3.Repositories
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }

        public virtual DbSet<Dealer> Dealers { get; set; }

        public virtual DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //setup composite key for Stock table
            modelBuilder.Entity<Stock>().HasKey(
                x => new { x.DealerId, x.CarId }
            );
        }
    }
}
