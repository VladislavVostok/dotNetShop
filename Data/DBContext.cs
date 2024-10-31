using dotNetShop.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Data
{
	public class DBContext: DbContext
	{
		public DbSet<ContactsFrom> Items { get; set; }
		public DbSet<Product> SerialNumbers { get; set; }
		public DbSet<Category> Categories { get; set; }

	}
}
