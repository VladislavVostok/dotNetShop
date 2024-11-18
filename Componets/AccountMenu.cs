using Microsoft.AspNetCore.Mvc;

namespace dotNetShop.Componets
{
    public class AccountMenu: ViewComponent
    {
        // TODO: Разобраться с компонентами 
		// https://csharp.webdelphi.ru/komponenty-predstavlenij-v-asp-net-core-mvc-viewcomponentresult-i-rabota-s-predstavleniyami/
		public IViewComponentResult Invoke()
        {
            return View("Default",HttpContext.User);
        }
    }
}
