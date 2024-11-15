using dotNetShop.ViewModels.ShopService;

namespace dotNetShop.Services
{
	public interface IShopService
	{
		Task<OverviewViewModel> GetShopAsync();
		Task<ProductCommentViewModel> GetProductByIdAsync(int id);
	}
}
