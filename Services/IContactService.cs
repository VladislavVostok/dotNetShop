using dotNetShop.Models;
using dotNetShop.ViewModels.ShopService;

namespace dotNetShop.Services
{
	public interface IContactService
	{
		Task<Contact> GetContactAsync();
	}
}
