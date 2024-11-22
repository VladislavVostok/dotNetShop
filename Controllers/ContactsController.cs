using dotNetShop.Data;
using dotNetShop.Models;
using dotNetShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Controllers
{
    public class ContactsController : Controller
    {
		private readonly IContactService _contactService;

		public ContactsController(IContactService contactService)
		{
			_contactService = contactService;
		}

		// GET: ContactsController
		public async Task<ActionResult> Index()
        {
			//ShopDBContext.SeedDataContact(_dbContext);

			Contact contact = await _contactService.GetContactAsync();

            return View(contact);
        }

    }
}
