using dotNetShop.Models;
using dotNetShop.Services;

namespace dotNetShop.ViewModels.ShopService
{
	public class OverviewViewModel
	{
		public List<BrandCountViewModel> Brands { get; set; }
		public List<ColorCountViewModel> Colors { get; set; }
		public List<CategoryCountViewModel> Categories { get; set; }
		public List<Product> Products { get; set; }
	}
}
