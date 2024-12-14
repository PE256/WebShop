using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;

namespace WebShop.Services;
public interface IUserService
{
    Task<IdentityUser> GetCurrentUserAsync(ControllerBase controller);
    Task<IdentityResult> CreateUserAsync(ControllerBase controller, UserFormModel user);
    Task<IList<string>> GetCurrentUserRolesAsync(ControllerBase controller);
    Task<UserInfo> GetCurrentUserInfoAsync(ControllerBase controller);
    Task<Microsoft.AspNetCore.Identity.SignInResult> SignInAsync(ControllerBase controller, UserFormModel user);
    Task SignOutAsync(ControllerBase controller);
}

