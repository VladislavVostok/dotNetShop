using dotNetShop.Models;
using dotNetShop.ViewModels.ContactService;

namespace dotNetShop.Services
{
	public interface IContactService
	{
		Task<Contact> GetContactAsync();
		Task SaveContactMessageAsync(FormContactMessageViewModel formContactMessageView);
	}
}
