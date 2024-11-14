using dotNetShop.Data;
using dotNetShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Controllers
{

	public class ProductCommentViewModel
	{
		public Product Product { get; set; }
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


	public class ShopController : Controller
	{

		private readonly ShopDBContext _context;

		public ShopController(ShopDBContext context)
		{
			_context = context;
		}

		// GET: ShopController
		public async Task<IActionResult> Index()
		{
			var brands = await _context.Brands
				.Include(b => b.Products) // Загрузка связанных продуктов
			.ToListAsync();

			var colors = await _context.Colors
				.Include(c => c.Products) // Загрузка связанных продуктов
			.ToListAsync();

			var categories = await _context.Categories
				.Include(c => c.Products) // Загрузка связанных продуктов
			.ToListAsync();

			var products = await _context.Products.ToListAsync();

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

			return View(viewModel);
		}

		// GET: ShopController/Details/5
		// GET: ProductComments/ProductDetails/{productId}
		public async Task<IActionResult> Details(int productId)
		{

			var product = await _context.Products
				.Include(p => p.Comments) // Загрузка связанных комментариев
				.FirstOrDefaultAsync(p => p.Id == productId);

			if (product == null)
			{
				return NotFound();
			}

			var viewModel = new ProductCommentViewModel
			{
				Product = product,
				NewComment = new Comment(), // Пустая модель для нового комментария
				Comments = product.Comments.OrderByDescending(c => c.CreatedAt).ToList()
			};

			return View(viewModel);
		}

        // POST: ProductComments/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(ProductCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.NewComment.ProductId = model.Product.Id;
                model.NewComment.CreatedAt = DateTime.UtcNow;

                _context.Comments.Add(model.NewComment);
                await _context.SaveChangesAsync();

                return RedirectToAction("ProductDetails", new { productId = model.Product.Id });
            }

            // Если валидация не прошла, снова загружаем комментарии
            model.Comments = await _context.Comments
                .Where(c => c.ProductId == model.Product.Id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View("ProductDetails", model);
        }
    }
}

