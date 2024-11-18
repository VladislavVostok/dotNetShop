using dotNetShop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace dotNetShop.Areas.Account.Models
{
    public class Login
    {

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [DisplayName("Логин")]
        public string LogIn { get; set; }

        [Required(ErrorMessage = "Поле Организация обязательно для заполнения")]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}