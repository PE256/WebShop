using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public AccountController(
            ILogger<HomeController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [NonAction]
        public async Task LogoutAsync()
        {
            
            await _userService.SignOutAsync(this);

            _logger.LogInformation("User logged out.");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await LogoutAsync();
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserFormModel user)
        {
            //Guard.IsNotNull(user, nameof(user));
            if (ModelState.IsValid)
            {
                var result = await _userService.SignInAsync(this, user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {user.Name} logged in.");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"{result}. Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await LogoutAsync();
            return View("Register");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserFormModel user)
        {
            if (ModelState.IsValid)
            {
                //
                var result = await _userService.CreateUserAsync(this, user);
                //

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            //If we got this far, something failed, redisplay form
            return View("Register");
        }
        public async Task<IActionResult> Logout()
        {
            await LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        public ViewResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}

