using dotNetShop.Models;
using dotNetShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotNetShop.Areas.API.Controllers
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { msg = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid username or password");

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            // Сохранение refresh-токена в базе данных или Redis
            await _userManager.SetAuthenticationTokenAsync(user, "JwtAuthDemo", "RefreshToken", refreshToken);

            SetRefreshTokenCookie(refreshToken);

            return Ok(new { accessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized("No refresh token provided");
         


			var username = User?.Identity?.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Unauthorized("User not found");

            var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "JwtAuthDemo", "RefreshToken");
            if (storedToken != refreshToken)
                return Unauthorized("Invalid refresh token");

            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = await _tokenService.GenerateRefreshToken();

            await _userManager.SetAuthenticationTokenAsync(user, "JwtAuthDemo", "RefreshToken", newRefreshToken);
            SetRefreshTokenCookie(newRefreshToken);

            return Ok(new { accessToken = newAccessToken });
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}

/*
 * 
Тестируйте регистрацию пользователя через /api/auth/register.

Авторизуйтесь через /api/auth/login.

Обновляйте токены через /api/auth/refresh.

*/