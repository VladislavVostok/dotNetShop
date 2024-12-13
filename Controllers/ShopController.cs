using dotNetShop.Data;
using dotNetShop.Services;
using Microsoft.AspNetCore.Mvc;
using dotNetShop.ViewModels.ShopService;

namespace dotNetShop.Controllers
{
	public class ShopController : Controller
	{

		private readonly IShopService _shopService;
		private readonly ShopDBContext _context;

		public ShopController(IShopService shopService,  ShopDBContext context)
		{
			_shopService = shopService;
			_context = context;	
		}

		// GET: ShopController
		public async Task<IActionResult> Index()
		{
			ShopDBContext.SeedDataShop(_context);
			ShopDBContext.SeedDataContact(_context);
			var viewModel = await _shopService.GetShopAsync();

			return View(viewModel);
		}

		// GET: ShopController/Details/5
		// GET: ProductComments/ProductDetails/{productId}
		public async Task<IActionResult> Details(int Id)
		{
            ProductCommentViewModel viewModel = await _shopService.GetProductByIdAsync(Id);

			if (viewModel.Product == null) return NotFound();
     
			return View(viewModel);
		}

		// POST: ProductComments/AddComment
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> AddComment(ProductCommentViewModel model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        model.NewComment.ProductId = model.Product.Id;
		//        model.NewComment.CreatedAt = DateTime.UtcNow;

		//        _context.Comments.Add(model.NewComment);
		//        await _context.SaveChangesAsync();

		//        return RedirectToAction("ProductDetails", new { productId = model.Product.Id });
		//    }

		//    // Если валидация не прошла, снова загружаем комментарии
		//    model.Comments = await _context.Comments
		//        .Where(c => c.ProductId == model.Product.Id)
		//        .OrderByDescending(c => c.CreatedAt)
		//        .ToListAsync();

		//    return View("ProductDetails", model);
		//}
	}
}

