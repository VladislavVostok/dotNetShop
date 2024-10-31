using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotNetShop.Controllers
{
	public class ShopController : Controller
	{
		// GET: ShopController
		public ActionResult Index()
		{
			return View();
		}

		// GET: ShopController/Details/5
		public ActionResult Details(int id)
		{
			return View();
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
