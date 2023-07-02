using Microsoft.EntityFrameworkCore;
using StockPulse.Models;

namespace StockPulse.Data
{
    public class Seeder
    {
        public void seed(ModelBuilder builder)
        {

            // Roles
            builder.Entity<Role>().HasData(new Role[]
            {
                new Role("Customer"),
                new Role("Employee"),
                new Role("Admin")
            });

        }
    }
}
