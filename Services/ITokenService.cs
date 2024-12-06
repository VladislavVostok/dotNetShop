using dotNetShop.Models;
using Microsoft.AspNetCore.Identity;

namespace dotNetShop.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateAccessToken(ApplicationUser user);
        public Task<string> GenerateRefreshToken();

    }
}
