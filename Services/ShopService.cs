using dotNetShop.Controllers;
using dotNetShop.Data;
using dotNetShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotNetShop.Services
{
	public class ShopService : IShopService
	{
		private readonly ShopDBContext _dbContext;

		public ShopService(ShopDBContext dbContext) { 
			_dbContext = dbContext;
		}

		public async Task<OverviewViewModel> GetShopAsync()
		{
			var brands = await _dbContext.Brands
				.Include(b => b.Products) // Загрузка связанных продуктов
			.ToListAsync();

			var colors = await _dbContext.Colors
				.Include(c => c.Products) // Загрузка связанных продуктов
			.ToListAsync();

			var categories = await _dbContext.Categories
				.Include(c => c.Products) // Загрузка связанных продуктов
			.ToListAsync();

			var products = await _dbContext.Products.ToListAsync();

			var viewModel = new OverviewViewModel
			{
				Brands = brands.Select(b => new BrandCountViewModel
				{
					BrandName = b.Name,
					ProductCount = b.Products.Count()
				}).ToList(),
				Colors = colors.Select(c => new ColorCountViewModel
				{
					ColorName = c.Name,
					ProductCount = c.Products.Count()
				}).ToList(),
				Categories = categories.Select(c => new CategoryCountViewModel
				{
					CategoryName = c.Name,
					ProductCount = c.Products.Count()
				}).ToList(),
				Products = products
			};

			return viewModel;
		}

		public async Task<ProductCommentViewModel> GetProductByIdAsync(int Id)
		{
			ProductCommentViewModel viewModel;

			
			var product = await _dbContext.Products
                .Include(p => p.Comments) // Загрузка связанных комментариев
				.Include(p => p.Brand)
				.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == Id);

			

            if (product == null)
            {
                viewModel = new ProductCommentViewModel
                {
                    Product = null,
					Brand = null,
                    Category = product.Category,
                    NewComment = null,
                    Comments = null
                };
            }

            viewModel = new ProductCommentViewModel
            {
                Product = product,
				Brand = product.Brand,
				Category = product.Category,
                NewComment = new Comment(), // Пустая модель для нового комментария
                Comments = product.Comments.OrderByDescending(c => c.CreatedAt).ToList()
            };

            return viewModel;
        }
    }

	public class ProductCommentViewModel
	{
		public Product Product { get; set; }
		public Brand Brand { get; set; }
		public Category Category { get; set; }
		public Comment NewComment { get; set; } // Модель для нового комментария 
		public List<Comment> Comments { get; set; } // Список всех комментариев к продукту }
	}
	public class OverviewViewModel
	{
		public List<BrandCountViewModel> Brands { get; set; }
		public List<ColorCountViewModel> Colors { get; set; }
		public List<CategoryCountViewModel> Categories { get; set; }
		public List<Product> Products { get; set; }
	}

	public class BrandCountViewModel
	{
		public string BrandName { get; set; }
		public int ProductCount { get; set; }
	}

	public class ColorCountViewModel
	{
		public string ColorName { get; set; }
		public int ProductCount { get; set; }
	}

	public class CategoryCountViewModel
	{
		public string CategoryName { get; set; }
		public int ProductCount { get; set; }
	}
}
