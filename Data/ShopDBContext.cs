using dotNetShop.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Data
{
	public class ShopDBContext: DbContext
	{
public ShopDBContext(DbContextOptions<ShopDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);
        }

		public DbSet<ContactsFrom> Items { get; set; }
		public DbSet<Product> SerialNumbers { get; set; }
		public DbSet<Category> Categories { get; set; }

	}
}
