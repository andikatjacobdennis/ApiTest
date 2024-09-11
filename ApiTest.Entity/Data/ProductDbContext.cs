using ApiTest.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Entity.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}
