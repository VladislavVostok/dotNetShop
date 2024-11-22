using dotNetShop.Data;
using dotNetShop.Models;
using dotNetShop.Services;
using dotNetShop.ViewModels.ContactService;
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


		[HttpGet]
		public async Task<ActionResult> Index()
        {
			//ShopDBContext.SeedDataContact(_dbContext);
			FormContactMessageViewModel formContactMessageViewModel = new FormContactMessageViewModel();
			formContactMessageViewModel.Contact = await _contactService.GetContactAsync();
			formContactMessageViewModel.FormContactMessage = new FormContactMessage();

            return View(formContactMessageViewModel);
        }

		[HttpPost]
		public async Task<ActionResult> Index(FormContactMessageViewModel contact)
		{
			if (contact.FormContactMessage.Name == null)
			{
				return RedirectToAction("Index", "Contacts");
			}

			_contactService.SaveContactMessageAsync(contact);


			return RedirectToAction("Index", "Contacts");
		}

	}
}
