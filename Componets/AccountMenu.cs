using Microsoft.AspNetCore.Mvc;

namespace dotNetShop.Componets
{
    public class AccountMenuViewComponent : ViewComponent
    {
        // TODO: Разобраться с компонентами 
		// https://csharp.webdelphi.ru/komponenty-predstavlenij-v-asp-net-core-mvc-viewcomponentresult-i-rabota-s-predstavleniyami/
		public IViewComponentResult Invoke()
        {
            return View("AccountMenu", HttpContext.User);
        }
    }
}
