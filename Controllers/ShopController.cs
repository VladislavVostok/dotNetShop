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
		public async Task<IActionResult> ProductDetails(int productId)
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

		// GET: ShopController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: ShopController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ShopController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ShopController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ShopController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ShopController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}

