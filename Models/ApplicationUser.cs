using Microsoft.AspNetCore.Identity;

namespace dotNetShop.Models
{
	public class ApplicationUser: IdentityUser<int>
	{
		public string Bio { get; set; }
		public string Adress { get; set; }
		public DateOnly Birthday { get; set; }


	}
}
