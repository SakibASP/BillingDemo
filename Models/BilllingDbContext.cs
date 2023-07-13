using Microsoft.EntityFrameworkCore;

namespace BillingDemo.Models
{
    public class BilllingDbContext : DbContext
    {
        public BilllingDbContext()
        {
        }

        public BilllingDbContext(DbContextOptions<BilllingDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Inventories> Inventories { get; set; }
        public virtual DbSet<InventoryProducts> InventoryProducts { get; set; }
    }
}
