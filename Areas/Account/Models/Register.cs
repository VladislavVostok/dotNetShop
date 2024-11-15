using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using dotNetShop.Models;

namespace dotNetShop.Areas.Account.Models
{
    public class Register
    {

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [DisplayName("Логин")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Поле E-mail обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Введен некорректный адрес")]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Поле Организация обязательно для заполнения")]
        [DisplayName("Организация")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Поле Пароль ещё раз обязательно для заполнения")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DisplayName("Пароль ещё раз")]
        public string ConfirmPassword { get; set; }

        public ApplicationUser GetUser()
        {
            ApplicationUser user = new()
            {
                Email = Email,
                UserName = Login,
            };
            return user;
        }
    }
}
