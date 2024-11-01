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
	}
}
