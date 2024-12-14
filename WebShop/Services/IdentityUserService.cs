using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Data;

namespace WebShop.Services;
public class IdentityUserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRoleStore<IdentityRole> _roleStore;
    private readonly SignInManager<IdentityUser> _signInManager;
    private ApplicationDbContext _dbContext;

    public IdentityUserService(
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        RoleManager<IdentityRole> roleManager,
        IRoleStore<IdentityRole> roleStore,
        SignInManager<IdentityUser> signInManager,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _userStore = userStore;
        _roleManager = roleManager;
        _roleStore = roleStore;
        _signInManager = signInManager;
        _dbContext = dbContext;

        var role1 = new IdentityRole { Name = "Customer" };
        var roleExists = _roleManager.RoleExistsAsync(role1.Name).Result;
        if (!roleExists)
        {
            _roleManager.CreateAsync(role1);
        }
        var role2 = new IdentityRole { Name = "Vendor" };
        roleExists = _roleManager.RoleExistsAsync(role2.Name).Result;
        if (!roleExists)
        {
            _roleManager.CreateAsync(role2);
        }
    }
    public async Task<IList<string>> GetCurrentUserRolesAsync(ControllerBase controller)
    {
        var user = await GetCurrentUserAsync(controller);
        IList<string> roles = null!;
        if (user != null) roles = await _userManager.GetRolesAsync(user);
        return roles;
    }
    public async Task<UserInfo> GetCurrentUserInfoAsync(ControllerBase controller)
    {
        var user = await GetCurrentUserAsync(controller);
        var info = _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefault();
        return info!;
    }
    public async Task<IdentityUser> GetCurrentUserAsync(ControllerBase controller)
    {
        var principal = controller.User;
        var user = await _userManager.GetUserAsync(principal);
        return user!;
    }
    public async Task<IdentityResult> CreateUserAsync(ControllerBase controller, UserFormModel user)
    {
        IdentityUser? identityUser;
        try
        {
            identityUser = Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, " +
                $"or alternatively override the register page");
        }

        identityUser.UserName = user.Name;
        await _userStore.SetUserNameAsync(identityUser, user.Name, CancellationToken.None);
        var result = await _userManager.CreateAsync(identityUser, user.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(identityUser, user.Role);

            await AuthenticateAsync(identityUser.Id, controller);
            await _signInManager.SignInAsync(identityUser, isPersistent: false);

            await _dbContext.Users.AddAsync(new UserInfo(identityUser.Id, identityUser.UserName, 0));
            await _dbContext.SaveChangesAsync();

        }
        return result;
    }
    public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInAsync(ControllerBase controller, UserFormModel user)
    {
        var result = await _signInManager.PasswordSignInAsync(
                    user.Name,
                    user.Password,
                    isPersistent: true,
                    lockoutOnFailure: false);
        if (result.Succeeded) 
        { 
            var principal = controller.User;
            await controller.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        return result;
    }
    async Task AuthenticateAsync(string id, ControllerBase controller)
    {
        // создаем один claim
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
            };
        // создаем объект ClaimsIdentity
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // установка аутентификационных куки
        await controller.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
    public async Task SignOutAsync(ControllerBase controller)
    {
        // Clear the existing external cookie to ensure a clean login process
        DeleteCookies(controller);
        await _signInManager.SignOutAsync();
    }
    void DeleteCookies(ControllerBase controller)
    {
        foreach (var cookie in controller.HttpContext.Request.Cookies)
        {
            if (cookie.Key.Contains("Cookies"))
                controller.Response.Cookies.Delete(cookie.Key);
        }
    }
}
