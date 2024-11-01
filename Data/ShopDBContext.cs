using dotNetShop.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Data
{
	public class ShopDBContext : DbContext
	{
		public ShopDBContext(DbContextOptions<ShopDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
			.HasOne(p => p.Category)
			.WithMany(p => p.Products)
			.HasForeignKey(p => p.CategoryId)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь многие-ко-многим: Product и Color
            modelBuilder.Entity<Product>()
                .HasMany(p => p.AvailableColors)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductColors"));

            // Связь один-ко-многим: Product и ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один-ко-многим: Product и Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Category>().HasData(
				new Product {Name="Кроссовки"}
			);

			modelBuilder.Entity<Brand>().HasData(
				new Product {Name="Addidas"},
				new Product {Name="Puma"}
			);


			modelBuilder.Entity<Product>().HasData(
				new Product {Name="addidas New Hammer sole for Sports person", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=1 },
				new Product {Name="ddsfjaskdfjlkasdjg", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=1 },
				new Product {Name="ddsfjaskdfjlkasdjg", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=2 },
				new Product {Name="ddsfjaskdfjlkasdjg", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=2 },
				new Product {Name="ddsfjaskdfjlkasdjg", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=1 },
				new Product {Name="ddsfjaskdfjlkasdjg", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=1 },
				new Product {Name="ddsfjaskdfjlkasdjg", Price=210.00M, DiscountedPrice=150.00M, CategoryId = 1, BrandId=1 }
			);

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Category> Categories { get; set;}
		public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Comment> Comments { get; set; }

	}
}
