using Microsoft.EntityFrameworkCore;
using StockPulse.Models;

namespace StockPulse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductStock>().HasKey(p => new
            {
                p.ProductNum,
                p.WareHouseId
            });

            modelBuilder.Entity<OrderLine>().HasKey(o => new
            {
                o.ProductNum,
                o.OrderNum
            });

            modelBuilder.Entity<EmployeeRole>().HasKey(p => new
            {
                p.EmployeeEmail,
                p.Name
            });

            Seeder seeder = new Seeder();
            seeder.seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public bool IsEmailUsed(string email)
        {
            return this.Persons.Any(p => p.Email == email);
        }

    }
}
