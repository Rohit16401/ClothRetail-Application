using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RetailUI.DBContext
{
    public class RetailUIDbContext : DbContext
    {
        public DbSet<Items> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace with your actual connection string
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-U3QDL1CQ\SQLEXPRESS;Initial Catalog=RetailUIDB;Integrated Security=True;Encrypt=False");
        }
    }
}
