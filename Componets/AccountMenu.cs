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


    public class UsersListViewComponent : ViewComponent
    {
        List<string> users = new()
        {
            "Tom", "Tim", "Bob", "Sam", "Alice", "Kate"
        };
        public IViewComponentResult Invoke()
        {
            int number = users.Count;
            // если передан параметр number
            if (Request.Query.ContainsKey("number"))
            {
                int.TryParse(Request.Query["number"], out number);
            }

            ViewBag.Users = users.Take(number);
            ViewData["Header"] = $"Количество пользователей: {number}.";
            return View();
        }
    }
}
