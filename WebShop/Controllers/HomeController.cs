using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(
            ILogger<HomeController> logger,
            IUserService userService,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userService = userService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            return View("Index");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Privacy()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            return View("Privacy");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Catalog()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            return View("Catalog");
        }
        public async Task<IActionResult> Basket()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            return View("Basket");
        }
        public async Task<IActionResult> Statistics()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            return View("Statistics");
        }
        public async Task<IActionResult> Profile()
        {
            var roles = await _userService.GetCurrentUserRolesAsync(this);
            ViewBag.Roles = roles;
            var info = await _userService.GetCurrentUserInfoAsync(this);

            var personalContent = new ProfileModel(info.Name, info.Coins.ToString("N2"));

            return View("Profile", model: personalContent);
        }
    }
}
